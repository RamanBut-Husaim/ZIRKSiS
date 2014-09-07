using System.Security.Cryptography;

namespace CryptoHash.Hashing
{
    public interface IHashingManagerBuilder
    {
        IHashingManager Build(HashAlgorithm hashAlgorithm, IPasswordProvider passwordProvider);
        IHashingManager BuildSecure(HMAC secureHashAlgorithm, IPasswordProvider passwordProvider);
    }
}
