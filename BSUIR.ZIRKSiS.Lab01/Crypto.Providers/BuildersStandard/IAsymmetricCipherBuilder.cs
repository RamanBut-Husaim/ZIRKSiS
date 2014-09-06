using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    public interface IAsymmetricCipherBuilder
    {
        AsymmetricAlgorithm Build();
    }
}
