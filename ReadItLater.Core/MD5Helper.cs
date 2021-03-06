using System;
using System.Security.Cryptography;
using System.Text;

namespace ReadItLater.Core
{
    public class MD5Helper
    {
        public static string Hash(string data)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(data);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            return Convert.ToBase64String(hash);
        }

        public static bool Verify(string data, string hash)
        {
            return 0 == StringComparer.OrdinalIgnoreCase.Compare(Hash(data), hash);
        }
    }
}
