using System.Collections.Generic;
using System.Security.Cryptography;
using CryptoHash.Providers.Builders;

namespace CryptoHash.Providers
{
    internal sealed class HashAlgorithmProvider : IHashAlgorithmProvider
    {
        private static readonly Dictionary<StandardHashAlgorithm, IHashAlgorithmBuilder> HashAlgorithmBuilders;
        private static readonly Dictionary<HMACHashAlgorithm, ISecureHashAlgorithmBuilder> SecureHashAlgorithmBuilders; 

        static HashAlgorithmProvider()
        {
            HashAlgorithmBuilders = new Dictionary<StandardHashAlgorithm, IHashAlgorithmBuilder>(3)
            {
                {StandardHashAlgorithm.SHA1, new SHA1HashBuilder()},
                {StandardHashAlgorithm.SHA512, new SHA512HashBuilder()},
                {StandardHashAlgorithm.MD5, new MD5HashBuilder()}
            };
            SecureHashAlgorithmBuilders = new Dictionary<HMACHashAlgorithm, ISecureHashAlgorithmBuilder>(3)
            {
                {HMACHashAlgorithm.HMACMD5, new MD5SecureHashBuilder()},
                {HMACHashAlgorithm.HMACSHA1, new SHA1SecureHashBuilder()},
                {HMACHashAlgorithm.HMACSHA512, new SHA512SecureHashBuilder()}
            };
        }

        public HashAlgorithm GetHashAlgorithm(StandardHashAlgorithm hashAlgorithm)
        {
            return HashAlgorithmBuilders[hashAlgorithm].Build();
        }

        public HMAC GetSecureHashAlgorithm(HMACHashAlgorithm hashAlgorithm)
        {
            return SecureHashAlgorithmBuilders[hashAlgorithm].Build();
        }
    }
}