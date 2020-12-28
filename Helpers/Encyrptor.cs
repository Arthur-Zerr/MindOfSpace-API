// using System.Text;
// using System;
// using System.Security.Cryptography;
// using Microsoft.Extensions.Configuration;
// using System.IO;
// using System.Linq;

// namespace MindOfSpace_Api.Helpers
// {
//     public class Encyrptor
//     {
//         protected int Keysize = 256;
//         private readonly IConfiguration configurator;
//         public Encyrptor(IConfiguration configurator)
//         {
//             this.configurator = configurator;
//         }

//         private string Encrypt(string password, string dateString)
//         {
//             var settingsSalt = configurator.GetSection("Encrypt:Salt").Value;
//             var passwordSalt = (password + dateString + settingsSalt).Replace(" ", "");
//             var saltStringBytes = Encoding.UTF8.GetBytes(passwordSalt);
//             var plainTextBytes = Encoding.UTF8.GetBytes(password);

//             using (var passwordBytes = new Rfc2898DeriveBytes(plainTextBytes, saltStringBytes, 20))
//             {
//                 var keyBytes = passwordBytes.GetBytes(Keysize / 8);
//                 using (var symmetricKey = new RijndaelManaged())
//                 {
//                     symmetricKey.BlockSize = 128;
//                     symmetricKey.Mode = CipherMode.CBC;
//                     symmetricKey.Padding = PaddingMode.PKCS7;
//                     using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8")))
//                     {
//                         using (var memoryStream = new MemoryStream())
//                         {
//                             using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
//                             {
//                                 cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
//                                 cryptoStream.FlushFinalBlock();

//                                 var cipherTextBytes = saltStringBytes;
//                                 cipherTextBytes = cipherTextBytes.Concat(saltStringBytes).ToArray();
//                                 cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
//                                 memoryStream.Close();
//                                 cryptoStream.Close();
//                                 return Convert.ToBase64String(cipherTextBytes);
//                             }
//                         }
//                     }
//                 }
//             }
//         }

//         public string EncryptPassword(string password, string dateSalt)
//         {
//             return Encrypt(password, dateSalt);
//         }

//         public string EncryptPassword(string password, DateTimeOffset date)
//         {
//             var dateSalt = date.ToString("u");

//             return Encrypt(password, dateSalt);
//         }
//     }
// }