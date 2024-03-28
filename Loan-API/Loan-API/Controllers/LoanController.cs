using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Loan_API.Domain;
using Loan_API.Data;
using Microsoft.AspNetCore.Authorization;
using Loan_API.Services;
using Loan_API.Models;
using Loan_API.Helpers;
using Microsoft.Extensions.Logging;

namespace Loan_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class LoanController : ControllerBase
    {
        private readonly UserContext _context;
        private ILoanService _loanService;
        private readonly ILogger<LoanController> _logger;
        private readonly IUserService _userService;

        public LoanController(UserContext context, ILoanService loanService, ILogger<LoanController> logger, IUserService userservice)
        {
            _context = context;
            _loanService = loanService;
            _logger = logger;
            _userService = userservice;
        }


        [Authorize(Roles = Roles.User)]
        [HttpPost("newloan")]
        public IActionResult AddLoan(AddLoanModel addLoanModel)
        {
            LoanValidator validator = new LoanValidator(_context);
            var result = validator.Validate(addLoanModel);
            if (!result.IsValid)
            {
                foreach (var error in (ValidationErrorParse.GetErrors(result)))
                {
                    _logger.LogError(error);
                }
                return BadRequest(ValidationErrorParse.GetErrors(result));  
            }
            var userId = _userService.GetOwnData().Id;
            if (_context.Users.Find(userId).IsBlocked == true) 
            {
                _logger.LogError("User blocked");
                return Unauthorized("The user is blocked"); 
            }
            _loanService.AddLoan(addLoanModel, userId);
            return Ok("Loan Created");
            
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("ownloans")]
        public IActionResult GetOwnLoans()
        {
            var userId = _userService.GetOwnData().Id; 
            return Ok(_loanService.GetOwnLoans(userId));
        }

        [Authorize(Roles = Roles.User)]
        [HttpPut("updateownloan")]
        public async Task <IActionResult> UpdateOwnLoan(UpdateLoanModel model)
        {
            LoanValidator validator = new LoanValidator(_context);
            var userId = _userService.GetOwnData().Id;
            var tempLoan = _loanService.UpdateOwnLoan(model).Result;
            tempLoan.UserId = userId;
            if (tempLoan.UserId != _context.Loans.Find(model.LoanId).UserId) 
            {
                _logger.LogError("Not own loan");
                return Unauthorized("You are not allowed to modify this loan. Reason: Not your loan"); 
            }
            if (tempLoan.Status != LoanStatus.Processing)
            {
                _logger.LogError("Loan already processed");
                return Unauthorized("You are not allowed to modify this loan. Reason: Loan already processed");
            }
            var verifiableLoan = validator.ConvertToValidatable(tempLoan);
            var result = validator.Validate(verifiableLoan);
            if (!result.IsValid)
            {
                return BadRequest(ValidationErrorParse.GetErrors(result));
            }
            _context.Loans.Update(tempLoan);
            await _context.SaveChangesAsync();
            return Ok("Loan Updated");
        }

        [Authorize(Roles = Roles.User)]
        [HttpDelete("deleteownloan")]
        public async Task<IActionResult> DeleteOwnLoan(LoanIdModel model)
        {
            var userId = _userService.GetOwnData().Id;
            IQueryable<Loan> ownLoans = _loanService.GetOwnLoans(userId);
            var loanToCheck = ownLoans.Where(loan => loan.Id == model.LoanId).FirstOrDefault();
            if (loanToCheck == null)
            {
                _logger.LogError("Loan not found");
                return UnprocessableEntity("Loan not found");
            }
            if (loanToCheck.Status != LoanStatus.Processing)
            {
                _logger.LogError("Loan already processed");
                return Unauthorized("You are not allowed to modify this loan. Reason: Loan already processed");
            }
            _loanService.DeleteOwnLoan(model.LoanId);
            return Ok("Loan Deleted");
        }


    }
}
