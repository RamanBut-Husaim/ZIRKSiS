using System.Security.Cryptography;

namespace CryptoHash.Providers.Builders
{
    internal sealed class SHA512SecureHashBuilder : ISecureHashAlgorithmBuilder
    {
        public HMAC Build()
        {
            return new HMACSHA512();
        }
    }
}
