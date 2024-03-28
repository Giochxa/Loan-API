using FluentValidation;
using Loan_API.Data;
using Loan_API.Domain;
using Loan_API.Models;
using System.Collections.Generic;


namespace Loan_API.Services
{

    public class LoanValidator : AbstractValidator<AddLoanModel>
    {
        
        public LoanValidator(UserContext context)
        {
            UserContext _context;
            _context = context;

            RuleFor(AddLoanModel => AddLoanModel.LoanType)
                .NotEmpty().NotNull().WithMessage("Loan Type is required")
                .Must(CheckLoanType).WithMessage($"Invalid Loan Type. Loan Type must be one of the following: {LoanType.QuickLoan}, " +
                                                                                                        $"{LoanType.CarLoan}, " +
                                                                                                        $"{LoanType.Installment}");
            RuleFor(AddLoanModel => AddLoanModel.Currency)
                .NotEmpty().NotNull().WithMessage("Currency is required")
                .Must(CheckCurrency).WithMessage($"Invalid Currency. Currency must be one of the following: {LoanCurrency.GEL}, " +
                                                                                                        $"{LoanCurrency.USD}, " +
                                                                                                        $"{LoanCurrency.EUR}");
            RuleFor(AddLoanModel => AddLoanModel.Amount)
                .NotNull().NotEmpty().WithMessage("Amount is required")
                .GreaterThan(500).WithMessage("Minimum amount is 500")
                .LessThan(50000).WithMessage("Visit the bank for requesting a loan higher than 50000");
            RuleFor(AddLoanModel => AddLoanModel.LoanPeriod)
                .NotNull().NotEmpty().WithMessage("Period is required")
                .GreaterThan(0).WithMessage("Period must be at least 1 month")
                .LessThan(120).WithMessage("Maximum loan period is 10 years");



        }

        private bool CheckLoanType(string loanType)
        {
            List<string> loanTypes = new List<string>()
                {
                    LoanType.QuickLoan.ToLower(),
                    LoanType.CarLoan.ToLower(),
                    LoanType.Installment.ToLower()
                };
            var loanTypeLower = loanType.ToLower();
            if (loanTypes.Contains(loanTypeLower)) return true;
            else return false;
        }

        private bool CheckCurrency(string currency)
        {
            List<string> currencies = new List<string>()
                {
                    LoanCurrency.USD.ToLower(),
                    LoanCurrency.EUR.ToLower(),
                    LoanCurrency.GEL.ToLower()
                };
            var currencyLower = currency.ToLower();
            if (currencies.Contains(currencyLower)) return true;
            else return false;
        }

        public AddLoanModel ConvertToValidatable(Loan model)
        {
            AddLoanModel addModel = new AddLoanModel();
            addModel.LoanType = model.Type;
            addModel.LoanPeriod = model.Period;
            addModel.Currency = model.Currency;
            addModel.Amount = model.Amount;
            return addModel;
        }
    }
}
