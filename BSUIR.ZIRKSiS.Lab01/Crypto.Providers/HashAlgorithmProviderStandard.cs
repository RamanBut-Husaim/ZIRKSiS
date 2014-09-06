using System.Collections.Generic;
using System.Security.Cryptography;
using Crypto.Providers.BuildersStandard;

namespace Crypto.Providers
{
    internal sealed class HashAlgorithmProviderStandard : IHashAlgorithmProviderStandard
    {
        private static readonly Dictionary<HashAlgorithms, IHashAlgorithmBuilder> HashAlgorithmProviders;

        static HashAlgorithmProviderStandard()
        {
            HashAlgorithmProviders = new Dictionary<HashAlgorithms, IHashAlgorithmBuilder>(1)
            {
                { HashAlgorithms.SHA1, new SHA1AlgorithmBuilder() },
                { HashAlgorithms.SHA512, new SHA512AlgorithmBuilder() }
            };
        }

        public HashAlgorithm GetHashAlgorithm(HashAlgorithms hashAlgorithm)
        {
            return HashAlgorithmProviders[hashAlgorithm].Build();
        }
    }
}
