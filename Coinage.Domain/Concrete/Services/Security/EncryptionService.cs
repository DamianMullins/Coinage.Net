using System.IO;
using Coinage.Domain.Abstract.Security;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Coinage.Domain.Concrete.Services.Security
{
    public class EncryptionService : IEncryptionService
    {
        public string CreateSaltKey(int size)
        {
            var rng = new RNGCryptoServiceProvider();
            var data = new byte[size];
            rng.GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public string CreatePasswordHash(string password, string saltkey)
        {
            string saltAndPassword = string.Concat(password, saltkey);
            var algorithm = HashAlgorithm.Create("SHA1");

            if (algorithm == null)
            {
                throw new Exception("Unrecognized hash name");
            }

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        //public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        //{
        //    if (string.IsNullOrEmpty(plainText))
        //        return plainText;

        //    if (String.IsNullOrEmpty(encryptionPrivateKey))
        //    {
        //        //encryptionPrivateKey = _securitySettings.EncryptionKey;
        //        encryptionPrivateKey = "8787687687687686"; // TODO
        //    }

        //    var tDESalg = new TripleDESCryptoServiceProvider
        //    {
        //        Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16)),
        //        IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8))
        //    };
        //    byte[] encryptedBinary = EncryptTextToMemory(plainText, tDESalg.Key, tDESalg.IV);

        //    return Convert.ToBase64String(encryptedBinary);
        //}

        //private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
        //        {
        //            byte[] toEncrypt = new UnicodeEncoding().GetBytes(data);
        //            cs.Write(toEncrypt, 0, toEncrypt.Length);
        //            cs.FlushFinalBlock();
        //        }

        //        return ms.ToArray();
        //    }
        //}
    }
}
