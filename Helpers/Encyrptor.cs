using System.Text;
using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace MindOfSpace_Api.Helpers
{
    public class Encyrptor
    {
        protected int Keysize = 256;
        public Encyrptor(IConfiguration configurator)
        {
        }

        public string EncryptPassword(string password, DateTime date)
        {
            var passwordSalt = password + date.ToShortDateString();
            var saltStringBytes = Encoding.UTF8.GetBytes(passwordSalt);
            var plainTextBytes = Encoding.UTF8.GetBytes(password);

            using (var passwordBytes = new Rfc2898DeriveBytes(plainTextBytes, saltStringBytes, 20))
            {
                var keyBytes = passwordBytes.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, saltStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();

                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(saltStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

    }
}