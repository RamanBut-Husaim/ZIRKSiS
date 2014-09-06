using System.Security.Cryptography;

namespace Crypto.Providers
{
    public interface IHashAlgorithmProviderStandard
    {
        HashAlgorithm GetHashAlgorithm(HashAlgorithms hashAlgorithm);
    }
}
