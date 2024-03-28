using Loan_API.Data;
using Loan_API.Domain;
using Loan_API.Models;
using Loan_API.Helpers;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Loan_API.Services
{
    public interface IAccountantService
    {
        public Task<IQueryable<Loan>> GetAnyLoan(int userId);
        public Task<Loan> UpdateAnyLoan(UpdateLoanModel model);
        public Task<Loan> DeleteAnyLoan(int loanId);
        public Task<User> BlockUser(int userId);
        public Task<User> UnblockUser(int userId);
        public Task<User> GenerateAccountant();
    }

    public class AccountantService : IAccountantService
    {
        private UserContext _context;
        private readonly AppSettings _appSettings;
        private UserService _userService;
        public AccountantService(UserContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public async Task<User> BlockUser(int userId)
        {
            var tempUser = _context.Users.Find(userId);
            tempUser.IsBlocked = true;
            _context.Users.Update(tempUser);
            await _context.SaveChangesAsync();
            return tempUser;
        }
        public async Task<User> UnblockUser(int userId)
        {
            var tempUser = _context.Users.Find(userId);
            tempUser.IsBlocked = false;
            _context.Users.Update(tempUser);
            await _context.SaveChangesAsync();
            return tempUser;
        }

        public async Task<Loan> DeleteAnyLoan(int loanId)
        {

            var loanToDelete = _context.Loans.Find(loanId);
            _context.Loans.Remove(loanToDelete);
            await _context.SaveChangesAsync();
            return loanToDelete;
        }

        public async Task <IQueryable<Loan>> GetAnyLoan(int userId)
        {
            return _context.Loans.Where(loan => loan.UserId == userId);
        }

        public async Task<Loan> UpdateAnyLoan(UpdateLoanModel model)
        {
            var tempLoan = await _context.Loans.FindAsync(model.LoanId);
            if (model.LoanType != null) tempLoan.Type = model.LoanType;
            else tempLoan.Type = _context.Loans.Where(loan => loan.Id == model.LoanId).FirstOrDefault().Type;
            if (model.Currency != null) tempLoan.Currency = model.Currency;
            else tempLoan.Currency = _context.Loans.Where(loan => loan.Id == model.LoanId).FirstOrDefault().Currency;
            if (model.Amount != 0) tempLoan.Amount = model.Amount;
            else tempLoan.Amount = _context.Loans.Where(loan => loan.Id == model.LoanId).FirstOrDefault().Amount;
            if (model.LoanPeriod != 0) tempLoan.Period = model.LoanPeriod;
            else tempLoan.Period = _context.Loans.Where(loan => loan.Id == model.LoanId).FirstOrDefault().Period;
            return tempLoan;
        }

        public async Task<User> GenerateAccountant()
        {
            var accountant = new User()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Age = 30,
                Salary = 999,
                UserName = "Accountant",
                Password = HashService.HashPassword("12345"),
                Role = "Accountant",
                IsBlocked = false
            };
            await _context.Users.AddAsync(accountant);
            await _context.SaveChangesAsync();
            return accountant;

        }
    }
}
