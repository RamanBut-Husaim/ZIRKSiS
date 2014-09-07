using System.Security.Cryptography;

namespace CryptoHash.Hashing
{
    public sealed class HashingManagerBuilder : IHashingManagerBuilder
    {
        public IHashingManager Build(HashAlgorithm hashAlgorithm, IPasswordProvider passwordProvider)
        {
            return new HashingManager(hashAlgorithm, passwordProvider);
        }

        public IHashingManager BuildSecure(HMAC secureHashAlgorithm, IPasswordProvider passwordProvider)
        {
            return new SecureHashingManager(secureHashAlgorithm, passwordProvider);
        }
    }
}
