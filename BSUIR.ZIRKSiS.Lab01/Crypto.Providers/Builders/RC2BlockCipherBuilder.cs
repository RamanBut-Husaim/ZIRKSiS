using Raksha.Crypto;
using Raksha.Crypto.Engines;

namespace Crypto.Providers.Builders
{
    internal sealed class RC2BlockCipherBuilder : IBlockCipherBuilder
    {
        public IBlockCipher Build()
        {
            return new RC2Engine();
        }
    }
}
