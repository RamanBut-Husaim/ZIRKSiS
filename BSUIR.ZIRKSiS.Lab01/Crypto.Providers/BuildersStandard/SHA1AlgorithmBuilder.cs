using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    internal sealed class SHA1AlgorithmBuilder : IHashAlgorithmBuilder
    {
        public HashAlgorithm Build()
        {
            return new SHA1CryptoServiceProvider();
        }
    }
}
