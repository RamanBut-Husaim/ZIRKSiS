using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    internal sealed class TripleDESCipherBuilder : ISymmetricCipherBuilder
    {
        public SymmetricAlgorithm Build()
        {
            return new TripleDESCryptoServiceProvider();
        }
    }
}
