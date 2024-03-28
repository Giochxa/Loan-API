using FluentValidation;
using Loan_API.Data;
using Loan_API.Models;
using System.Linq;

namespace Loan_API.Services
{
    public class UserValidator : AbstractValidator<RegistrationModel>
    {
        UserContext _context;
        public UserValidator(UserContext context)
        {
            _context = context; 

            RuleFor(RegistrationModel => RegistrationModel.FirstName)
                .Length(1, 50).WithMessage("First Name must contain 1-50 characters")
                .NotNull().WithMessage("First Name is mandatory")
                .Matches(@"^[a-zA-Z]+$").WithMessage("First Name must only contain letters");
            RuleFor(RegistrationModel => RegistrationModel.LastName)
                .Length(1, 50).WithMessage("Last Name must containt 1-50 characters")
                .NotNull().WithMessage("Last Name is mandatory")
                .Matches(@"^[a-zA-Z]+$").WithMessage("Last Name must only contain letters");
            RuleFor(RegistrationModel => RegistrationModel.UserName)
                .Length(1, 50).WithMessage("Username must containt 1-50 characters")
                .NotNull().WithMessage("Username is mandatory")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Username must contain only letters or numbers")
                .Must(UniqueUserName).WithMessage("Username already exists");
            RuleFor(RegistrationModel => RegistrationModel.Password)
                .Length(8, 50).WithMessage("Password must contain 8-50 characters")
                .NotNull().WithMessage("Password is mandatory")
                .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
            RuleFor(RegistrationModel => RegistrationModel.Age)
                .NotNull().WithMessage("Age is Mandatory")
                .GreaterThan(18).WithMessage("Underage users not allowed")
                .LessThan(100).WithMessage("Age too high");
            RuleFor(RegistrationModel => RegistrationModel.Salary)
                .NotNull().WithMessage("Salary is mandatory")
                .GreaterThan(100).WithMessage("Minimum salary is 100")
                .LessThan(1000000).WithMessage("Salary too high");
        
    }

        private bool UniqueUserName(string name)
        {
            var uniqueCheck = _context.Users.Where(x => x.UserName.ToLower() == name.ToLower()).FirstOrDefault();

            if (uniqueCheck == null) return true;
            return false;
        }


    }
}
