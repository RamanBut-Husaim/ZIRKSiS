using System;
using System.Security.Cryptography;
using System.Text;

namespace Kerberos.Crypto
{
    public sealed class CryptoHelper
    {
        public static string GenerateSalt(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("The length parameter cannot be below zero!");
            }
            string resultString;
            var cryptoServiceProvider = new RNGCryptoServiceProvider();
            var saltBytes = new byte[length];
            cryptoServiceProvider.GetNonZeroBytes(saltBytes);
            resultString = Encoding.Unicode.GetString(saltBytes);
            return resultString;
        }

        public static string ComputePasswordHash(string password, string salt)
        {
            string resultString;
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt))
            {
                throw new ArgumentException("Password or salt cannot be null or empty!");
            }
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] passwordHash = SHA512.Create().ComputeHash(passwordBytes);
            string passwordHashString = Encoding.Unicode.GetString(passwordHash);
            string passwordSaltConcat = string.Concat(passwordHashString, salt);
            byte[] passwordSaltBytes = Encoding.Unicode.GetBytes(passwordSaltConcat);
            byte[] resultBytes = SHA512.Create().ComputeHash(passwordSaltBytes);
            resultString = Encoding.Unicode.GetString(resultBytes);
            return resultString;
        }
    }
}
