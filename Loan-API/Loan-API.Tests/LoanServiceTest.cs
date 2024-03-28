using System.Linq;
using Xunit;
using Loan_API.Tests.FakeServices;
using Loan_API.Models;
using System.Threading.Tasks;

namespace Loan_API.Tests
{
    public class LoanServiceTest : IClassFixture<FakeLoanService>
    {
        FakeLoanService _service;
        public LoanServiceTest(FakeLoanService service)
        {
            _service = service;
        }

        [Fact]
        public void LoanShouldNotBeNull()
        {
            var model = new AddLoanModel() { Amount = 1000, Currency = "USD", LoanPeriod = 10, LoanType = "Car Loan" };
            var userId = 1;

            var result = _service.AddLoan(model, userId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CorrectLoanShouldBeDeleted()
        {
            var loanId = 1;
            var deletedLoan = await _service.DeleteOwnLoan(loanId);

            // Check if the deletedLoan is not null before accessing its Id property
            Assert.NotNull(deletedLoan);

            // Check if the ID of the deleted loan matches the loanId provided for deletion
            Assert.Equal(loanId, deletedLoan.Id);
        }


        [Fact]
        public void GetLoanCountShouldBeOne()
        {
            var loanCount = 1;
            var fetchedLoans = _service.GetOwnLoans(1);

            Assert.Equal(fetchedLoans.Count(), loanCount);
        }

        [Fact]
        public void LoanShouldBeUpdated()
        {
            var model = new UpdateLoanModel() { Amount = 100, Currency = "GEL", LoanId = 1, LoanPeriod = 10, LoanType = "Car Loan" };
            var result = _service.UpdateOwnLoan(model).Result;
            Assert.Equal(model.Amount, result.Amount);
        }
    }
}
