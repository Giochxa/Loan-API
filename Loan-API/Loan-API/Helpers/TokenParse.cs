using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Loan_API.Helpers
{
    public interface ITokenParse
    {
        public int GetUserId();
    }
    public class TokenParse : ITokenParse
    {
        private readonly IHttpContextAccessor _httpContext;

        public TokenParse(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public int GetUserId()
        {
            var user = _httpContext?.HttpContext?.User as ClaimsPrincipal;
            var userId = user.Claims.ElementAt(0).Value;
            var userIdInt = Convert.ToInt32(userId);
            return userIdInt;
        }
    }


}
