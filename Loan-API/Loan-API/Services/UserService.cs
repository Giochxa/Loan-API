using Loan_API.Data;
using Loan_API.Domain;
using Loan_API.Models;
using Loan_API.Helpers;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

namespace Loan_API.Services
{
    public interface IUserService
    {
        public Task <User> Register(RegistrationModel user);
        User Authenticate(string username, string password);
        public Task<User> Login(User user);
        User GetOwnData();
        string GenerateToken(User user);
    }

    public class UserService : IUserService
    {
        private UserContext _context;
        private readonly AppSettings _appSettings;
        private readonly ITokenParse _tokenParse;
        public UserService(UserContext context, IOptions<AppSettings> appSettings, ITokenParse tokenParse)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _tokenParse = tokenParse;
        }

        public async Task <User> Register(RegistrationModel regData)
        {

            var user = new User();
            user.FirstName = regData.FirstName;
            user.LastName = regData.LastName;
            user.UserName = regData.UserName;
            user.Password = HashService.HashPassword(regData.Password);
            user.Age = regData.Age;
            user.Salary = regData.Salary;
            await _context.Users.AddAsync(user);
            return user;
        }

        public User Authenticate(string username, string password)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.UserName == username);

            if (user == null)
                return null;

            if (HashService.HashPassword(password) != user.Password)
                return null;

            return user;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async Task<User> Login(User user)
        {

            if (user == null) return null;
            string tokenString = GenerateToken(user);
            var userId = _context.Users
                        .Where(x => x.UserName == user.UserName)
                        .Select(x => x.Id)
                        .SingleOrDefault();
            var currentrecord = _context.Users.Find(userId);
            user.Token = tokenString;
            currentrecord.Token = tokenString;
            _context.Users.Update(currentrecord);
            await _context.SaveChangesAsync();
            return user;
        }


        public User GetOwnData()
        {
            var userId = _tokenParse.GetUserId();
            var currentUser = _context.Users.Find(userId);
            return currentUser;
        }
    }
}

