using System.Security.Cryptography;

namespace CryptoHash.Providers.Builders
{
    internal sealed class SHA1SecureHashBuilder : ISecureHashAlgorithmBuilder
    {
        public HMAC Build()
        {
            return new HMACSHA1();
        }
    }
}
