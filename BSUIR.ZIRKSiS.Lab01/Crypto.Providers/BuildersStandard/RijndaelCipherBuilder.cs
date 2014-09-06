using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    internal sealed class RijndaelCipherBuilder : ISymmetricCipherBuilder
    {
        public SymmetricAlgorithm Build()
        {
            return new RijndaelManaged();
        }
    }
}
