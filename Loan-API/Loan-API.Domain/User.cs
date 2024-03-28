using System.Collections.Generic;

namespace Loan_API.Domain
{
    public class User
    {
        public User()
        {
            IsBlocked = false;
            Role = Roles.User;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public bool IsBlocked { get; set; }
        public string Role { get; set; }
        public List<Loan> Loans { get; set; } = new List<Loan>();
        public string Token { get; set; }


    }
}
