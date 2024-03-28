using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Loan_API.Domain;
using Loan_API.Data;
using Loan_API.Services;
using Loan_API.Models;
using Loan_API.Helpers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Loan_API.Controllers
{
    [Authorize(Roles = Roles.Accountant)]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountantController : ControllerBase
    {
        private readonly UserContext _context;
        private IAccountantService _accountantService;
        private IUserService _userService;
        private readonly ILogger<AccountantController> _logger;
        public AccountantController(UserContext context, IAccountantService accountantService, IUserService userService,
            ILogger<AccountantController> logger)
        {
            _context = context;
            _accountantService = accountantService;
            _userService = userService;
            _logger = logger;
        }


        [HttpPut("blockuser")]
        public async Task<IActionResult> BlockUser(UserIdModel model)
        {
            if (_context.Users.Find(model.UserId) == null)
            {
                _logger.LogError("User not found");
                return UnprocessableEntity("User not found");
            }
            await _accountantService.BlockUser(model.UserId);
            return Ok("User Blocked");
        }

        [HttpPut("unblockuser")]
        public async Task<IActionResult> UnblockUser(UserIdModel model)
        {
            if (_context.Users.Find(model.UserId) == null)
            {
                _logger.LogError("User not found");
                return UnprocessableEntity("User not found");
            }
            await _accountantService.UnblockUser(model.UserId);
            return Ok("User Unblocked");
        }

        [HttpGet("anyuserloans")]
        public async Task<IActionResult> GetAnyUserLoans(UserIdModel model)
        {
            if (_context.Users.Find(model.UserId) == null)
            {
                _logger.LogError("User not found");
                return UnprocessableEntity("User not found");
            }
            var userLoans = await _accountantService.GetAnyLoan(model.UserId);
            return Ok(userLoans);
        }

        [HttpDelete("deleteanyloan")]
        public async Task<IActionResult> DeleteAnyLoan(LoanIdModel model)
        {
            if (_context.Loans.Find(model.LoanId) == null)
            {
                _logger.LogError("Loan not found");
                return UnprocessableEntity("Loan not found");
            }
            await _accountantService.DeleteAnyLoan(model.LoanId);
            return Ok("Loan Deleted");
        }

        [HttpPut("updateanyloan")]
        public async Task<IActionResult> UpdateAnyLoan(UpdateLoanModel model)
        {
            if (_context.Loans.Find(model.LoanId) == null)
            {
                _logger.LogError("Loan not found");
                return UnprocessableEntity("Loan not found");
            }
            LoanValidator validator = new LoanValidator(_context);
            var tempLoan = await _accountantService.UpdateAnyLoan(model);
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
        [AllowAnonymous]
        [HttpPost("generateaccountant")]
        public async Task<IActionResult> GenerateAccountant()
        {
            var accountant = await _accountantService.GenerateAccountant();
            var tokenString = _userService.GenerateToken(accountant);
            accountant.Token = tokenString;
            _context.Users.Update(accountant);
            _context.SaveChanges();
            return Ok($"Accountant created, your Credentials: Username: Accountant" +
                $"Password: 12345"+
                $"Token: {accountant.Token}");
        } 
    }
}
