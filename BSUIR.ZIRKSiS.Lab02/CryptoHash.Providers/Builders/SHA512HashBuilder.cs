using System.Security.Cryptography;

namespace CryptoHash.Providers.Builders
{
    internal sealed class SHA512HashBuilder : IHashAlgorithmBuilder
    {
        public HashAlgorithm Build()
        {
            return new SHA512CryptoServiceProvider();
        }
    }
}
