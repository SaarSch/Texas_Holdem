using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TexasHoldem
{
    public class Crypto
    {

        public static string Encrypt(string password)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return String.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(item => item.ToString("x2")));
            }
        }   
    }

}