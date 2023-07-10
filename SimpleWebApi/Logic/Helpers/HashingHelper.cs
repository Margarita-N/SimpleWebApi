using System.Security.Cryptography;
using System.Text;

namespace SimpleWebApi.Logic.Helpers
{
    public static class HashingHelper
    {
        public static string HashPassword(string password, string salt)
        {
            var combinedPassword = password + salt;

            var hashingModule = SHA256.Create();
            byte[] hashBytes = hashingModule.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));

            return Encoding.UTF8.GetString(hashBytes);
        }
    }
}
