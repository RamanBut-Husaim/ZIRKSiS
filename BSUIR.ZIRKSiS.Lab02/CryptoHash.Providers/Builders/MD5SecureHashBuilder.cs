using System.Security.Cryptography;

namespace CryptoHash.Providers.Builders
{
    internal sealed class MD5SecureHashBuilder : ISecureHashAlgorithmBuilder
    {
        public HMAC Build()
        {
            return new HMACMD5();
        }
    }
}
