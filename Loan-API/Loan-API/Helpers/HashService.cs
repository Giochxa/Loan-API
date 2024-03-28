using System;
using System.Security.Cryptography;
using System.Text;
namespace Loan_API.Helpers
    
{
    public class HashService
    {
        public static string HashPassword(string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            byte[] encryptedBytes = sha1.ComputeHash(passwordBytes);
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
