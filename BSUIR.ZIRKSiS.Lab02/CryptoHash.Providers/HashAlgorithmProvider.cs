using System.Collections.Generic;
using System.Security.Cryptography;
using CryptoHash.Providers.Builders;

namespace CryptoHash.Providers
{
    internal sealed class HashAlgorithmProvider : IHashAlgorithmProvider
    {
        private static readonly Dictionary<StandardHashAlgorithm, IHashAlgorithmBuilder> HashAlgorithmBuilders;

        static HashAlgorithmProvider()
        {
            HashAlgorithmBuilders = new Dictionary<StandardHashAlgorithm, IHashAlgorithmBuilder>(3)
            {
                {StandardHashAlgorithm.SHA1, new SHA1HashBuilder()},
                {StandardHashAlgorithm.SHA512, new SHA512HashBuilder()},
                {StandardHashAlgorithm.MD5, new MD5HashBuilder()}
            };
        }

        public HashAlgorithm GetHashAlgorithm(StandardHashAlgorithm hashAlgorithm)
        {
            return HashAlgorithmBuilders[hashAlgorithm].Build();
        }
    }
}