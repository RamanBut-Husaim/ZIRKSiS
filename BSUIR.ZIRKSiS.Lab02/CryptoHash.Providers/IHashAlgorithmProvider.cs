using System.Security.Cryptography;

namespace CryptoHash.Providers
{
    public interface IHashAlgorithmProvider
    {
        HashAlgorithm GetHashAlgorithm(StandardHashAlgorithm hashAlgorithm);
    }
}