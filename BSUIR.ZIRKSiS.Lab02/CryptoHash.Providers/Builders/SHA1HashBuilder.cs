using System.Security.Cryptography;

namespace CryptoHash.Providers.Builders
{
    internal sealed class SHA1HashBuilder : IHashAlgorithmBuilder
    {
        public HashAlgorithm Build()
        {
            return new SHA1CryptoServiceProvider();
        }
    }
}
