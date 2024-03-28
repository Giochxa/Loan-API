using Loan_API.Domain;
using Loan_API.Models;
using Loan_API.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loan_API.Tests.FakeServices
{
    public class FakeLoanService : ILoanService
    {
        public async Task<Loan> AddLoan(AddLoanModel loanModel, int userId)
        {
            var newLoan = new Loan();
            newLoan.UserId = userId;
            newLoan.Type = loanModel.LoanType;
            newLoan.Currency = loanModel.Currency;
            newLoan.Amount = loanModel.Amount;
            newLoan.Period = loanModel.LoanPeriod;
            return newLoan;
        }

        public async Task<Loan> DeleteOwnLoan(int loanId)
        {
            var loans = new List<Loan>() { new Loan() { Id = 1 }, new Loan() { Id = 2 } };
            var loanToDelete = loans.Where(Loan => Loan.Id == loanId).FirstOrDefault();
            loans.Remove(loanToDelete);
            return loanToDelete;
        }

        public IQueryable<Loan> GetOwnLoans(int userId)
        {
            var loans = new List<Loan>() { new Loan() { Id = 1, UserId = 1 }, new Loan() { Id = 2, UserId = 2 } };
            var loansToGet = loans.Where(loan => loan.UserId == userId).AsQueryable();
            return loansToGet;
        }

        public async Task<Loan> UpdateOwnLoan(UpdateLoanModel model)
        {
            var tempLoan = new Loan() { Id = model.LoanId };
            if (model.LoanType != null) tempLoan.Type = model.LoanType;
            if (model.Currency != null) tempLoan.Currency = model.Currency;
            if (model.Amount != 0) tempLoan.Amount = model.Amount;
            if (model.LoanPeriod != 0) tempLoan.Period = model.LoanPeriod;
            return tempLoan;
        }
    }
}
