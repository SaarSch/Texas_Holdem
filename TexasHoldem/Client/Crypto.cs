using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Client
{
    public class Crypto
    {
        private static readonly string PasswordHash = "P@@Sw0rd";
        private static readonly string SaltKey = "S@LT&KEY";
        private static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        public static string Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC, Padding = PaddingMode.Zeros};
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }

            var crypt = new StringBuilder(Convert.ToBase64String(cipherTextBytes));
            if (crypt.Length > 0)
                for (var i = 0; i < 3; i++)
                    if (crypt[crypt.Length - 1 - i] == '=')
                        crypt[crypt.Length - 1 - i] = '*';
                    else break;
            return crypt.ToString();
        }

        public static string Decrypt(string encryptedText)
        {
            encryptedText = encryptedText.Replace(' ', '+');
            var crypt = new StringBuilder(encryptedText);
            if (crypt.Length > 0)
                for (var i = 0; i < 3; i++)
                    if (crypt[crypt.Length - 1 - i] == '*')
                        crypt[crypt.Length - 1 - i] = '=';
                    else break;
            var cipherTextBytes = Convert.FromBase64String(crypt.ToString());
            var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC, Padding = PaddingMode.None};

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];

            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            var ans = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());


            return ans;
        }
    }
}