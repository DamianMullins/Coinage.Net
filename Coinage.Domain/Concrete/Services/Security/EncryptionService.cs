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
    }
}
