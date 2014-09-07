using System.Security.Cryptography;

namespace CryptoHash.Providers.Builders
{
    internal sealed class MD5HashBuilder : IHashAlgorithmBuilder
    {
        public HashAlgorithm Build()
        {
            return new MD5CryptoServiceProvider();
        }
    }
}
