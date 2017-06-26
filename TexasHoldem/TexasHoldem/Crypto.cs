using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TexasHoldem
{
    public class Crypto
    {
        public static string Encrypt(string password)
        {
            using (var hash = SHA256.Create())
            {
                return string.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(item => item.ToString("x2")));
            }
        }
    }
}