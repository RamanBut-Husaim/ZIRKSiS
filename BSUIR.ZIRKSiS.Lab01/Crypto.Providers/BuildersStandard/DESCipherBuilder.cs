using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    internal sealed class DESCipherBuilder : ISymmetricCipherBuilder
    {
        public SymmetricAlgorithm Build()
        {
            return new DESCryptoServiceProvider();
        }
    }
}
