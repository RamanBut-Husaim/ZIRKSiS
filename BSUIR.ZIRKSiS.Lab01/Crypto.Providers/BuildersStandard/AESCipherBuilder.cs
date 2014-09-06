using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    internal sealed class AESCipherBuilder : ISymmetricCipherBuilder
    {
        public SymmetricAlgorithm Build()
        {
            return new AesCryptoServiceProvider();
        }
    }
}
