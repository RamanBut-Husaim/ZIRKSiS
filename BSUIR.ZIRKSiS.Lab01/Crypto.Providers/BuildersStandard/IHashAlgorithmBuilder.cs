using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    public interface IHashAlgorithmBuilder
    {
        HashAlgorithm Build();
    }
}
