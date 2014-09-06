using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    public interface ISymmetricCipherBuilder
    {
        SymmetricAlgorithm Build();
    }
}
