using Loan_API.Domain;
using Loan_API.Helpers;
using Loan_API.Models;
using Loan_API.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loan_API.Tests.FakeServices
{
    public class FakeUserService : IUserService
    {
        public User Authenticate(string username, string password)
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
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;


            if (HashService.HashPassword(password) != tempUser.Password)
                return null;

            return tempUser;
        }

        public string GenerateToken(User user)
        {
            var token = "this is a token";
            return token;
        }

        public User GetOwnData()
        {
            var user = new User() { Id = 1 };
            var users = new List<User> { new User { Id = 1},
            new User{ Id = 2},
            new User {Id = 3}};
            var result = users.Where(user => user.Id == user.Id).FirstOrDefault();
            return result;
        }

        public async Task<User> Login(User user)
        {
            var tokenString = GenerateToken(user);
            user.Token = tokenString;
            return user;
        }

        public async Task<User> Register(RegistrationModel regData)
        {
            var user = new User();
            user.FirstName = regData.FirstName;
            user.LastName = regData.LastName;
            user.UserName = regData.UserName;
            user.Password = HashService.HashPassword(regData.Password);
            user.Age = regData.Age;
            user.Salary = regData.Salary;
            return user;
        }
    }
}
