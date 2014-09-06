using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    internal sealed class RC2CipherBuilder : ISymmetricCipherBuilder
    {
        public SymmetricAlgorithm Build()
        {
            return new RC2CryptoServiceProvider();
        }
    }
}
