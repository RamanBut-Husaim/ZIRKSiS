using Raksha.Crypto;
using Raksha.Crypto.Engines;

namespace Crypto.Providers.Builders
{
    internal sealed class AESBlockCipherBuilder : IBlockCipherBuilder
    {
        public IBlockCipher Build()
        {
            return new AesEngine();
        }
    }
}
