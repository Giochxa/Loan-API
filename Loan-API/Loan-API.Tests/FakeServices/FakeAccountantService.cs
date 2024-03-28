using Loan_API.Domain;
using Loan_API.Helpers;
using Loan_API.Models;
using Loan_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loan_API.Tests.FakeServices
{
    public class FakeAccountantService : IAccountantService
    {
        
        public async Task<User> BlockUser(int userId)
        {
            User tempUser = new User()
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Age = 30,
                Salary = 999,
                UserName = "Accountant",
                Password = HashService.HashPassword("12345"),
                Role = "Accountant",
                IsBlocked = false,
                Loans = new List<Loan> { new Loan { Id = 1},
            new Loan { Id = 2}}
            };
            tempUser.IsBlocked = true;
            return tempUser;
        }

        public async Task<Loan> DeleteAnyLoan(int loanId)
        {
            User tempUser = new User()
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Age = 30,
                Salary = 999,
                UserName = "Accountant",
                Password = HashService.HashPassword("12345"),
                Role = "Accountant",
                IsBlocked = false,
                Loans = new List<Loan> { new Loan { Id = 1},
            new Loan { Id = 2}}
            };
            var loanToDelete = tempUser.Loans.Where(loan => loan.Id == loanId).FirstOrDefault();
            tempUser.Loans.Remove(loanToDelete);
            return loanToDelete;
        }

        public async Task<User> GenerateAccountant()
        {
            var accountant = new User()
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Age = 30,
                Salary = 999,
                UserName = "Accountant",
                Password = HashService.HashPassword("12345"),
                Role = "Accountant",
                IsBlocked = false
            };

            return accountant;
        }

        public async Task<IQueryable<Loan>> GetAnyLoan(int userId)
        {
            var loans = new List<Loan> { new Loan { UserId = 1},
            new Loan{ UserId = 1},
            new Loan {UserId = 2}};
            var result = loans.Where(loan => loan.UserId == userId).ToList<Loan>();
            return result.AsQueryable();
        }

        public Task<User> UnblockUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Loan> UpdateAnyLoan(UpdateLoanModel model)
        {
            throw new NotImplementedException();
        }
    }
}
