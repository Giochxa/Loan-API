using System.Linq;
using Xunit;
using Loan_API.Domain;
using Loan_API.Helpers;
using System.Collections.Generic;
using Loan_API.Tests.FakeServices;

namespace Loan_API.Tests
{
    public class AccountantServiceTest : IClassFixture<FakeAccountantService>
    {
        FakeAccountantService _service;

        public AccountantServiceTest(FakeAccountantService service)
        {
            _service = service;
        }

        public List<User> accountants = new List<User> { new User
        {
            Id = 1,
            UserName = "Fake",
            FirstName = "Fake",
            LastName = "Fake",
            Password = HashService.HashPassword("12345"),
            Age = 20,
            Salary = 100,
            IsBlocked = false,
            Role = "Accountant",
            Loans = new List<Loan> { new Loan { Id = 1},
            new Loan { Id = 2}}
        } };

        [Fact]
        public void GenerateAccountantShouldBeSameAsPreset()
        {
            var acc = accountants[0];
            var result = _service.GenerateAccountant();
            Assert.Equal(acc.Id, result.Id);
        }

        [Fact]
        public void BlockedUserShouldNotBeSameAsBeforeBlock()
        {
            var acc = accountants[0];
            var result = _service.BlockUser(1).Result;
            Assert.NotEqual(acc.IsBlocked, result.IsBlocked);
        }

        [Fact]
        public void DeleteShouldBeNullWithNegativeId()
        {
            Loan falseLoan = null;
            var result = _service.DeleteAnyLoan(-1).Result;
            Assert.Equal(result, falseLoan);
        }

        [Fact]
        public void GetLoansSHouldReturnTwoLoans()
        {
            var loans = new List<Loan> { new Loan { UserId = 1},
            new Loan{ UserId = 1},
            new Loan {UserId = 2}};

            var result = _service.GetAnyLoan(1).Result;
            Assert.NotEqual(loans.Count(), result.Count());
            
        }


    }
}
