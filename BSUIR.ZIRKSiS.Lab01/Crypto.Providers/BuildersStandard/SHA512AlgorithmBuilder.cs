using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    internal sealed class SHA512AlgorithmBuilder : IHashAlgorithmBuilder
    {
        public HashAlgorithm Build()
        {
            return new SHA512CryptoServiceProvider();
        }
    }
}
