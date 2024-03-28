using Microsoft.EntityFrameworkCore;
using Loan_API.Domain;

namespace Loan_API.Data
{
    public class UserContext : DbContext
    {
        //public UserContext()
        //{
        //}

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Logs> Logs { get; set; }
    }
}
