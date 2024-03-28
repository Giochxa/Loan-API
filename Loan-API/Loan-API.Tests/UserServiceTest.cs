using Xunit;
using Loan_API.Domain;
using Loan_API.Tests.FakeServices;
using Loan_API.Models;

namespace Loan_API.Tests
{
    public class UserServiceTest : IClassFixture<FakeUserService>
    {
        FakeUserService _service;
        public UserServiceTest(FakeUserService service)
        {
            _service = service;
        }

        [Fact]
        public void ShouldBeNullIfCredsAreNull()
        {
            var username = "";
            var password = "";

            var result = _service.Authenticate(username, password);
            Assert.Null(result);
        }

        [Fact]
        public void GetUserResultShouldBeEqualToUser()
        {
            var idToCompare = 1;
            var result = _service.GetOwnData();
            Assert.Equal(result.Id, idToCompare);

        }
        [Fact]
        public void TokenShouldBeGenerated()
        {
            var user = new User() { Id = 1, Role = "User" };
            _service.Login(user);

            Assert.NotNull(user.Token);
        }

        [Fact]
        public void UserShouldBeGenerated()
        {
            RegistrationModel model = new RegistrationModel() { Age = 10, FirstName = "test", LastName = "test", Password = "test" };
            var result = _service.Register(model);
            Assert.NotNull(result);
        }
    }
}
