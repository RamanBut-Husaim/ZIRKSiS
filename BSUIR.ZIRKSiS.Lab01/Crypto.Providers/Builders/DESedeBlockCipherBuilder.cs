using Raksha.Crypto;
using Raksha.Crypto.Engines;

namespace Crypto.Providers.Builders
{
    internal sealed class DESedeBlockCipherBuilder : IBlockCipherBuilder
    {
        public IBlockCipher Build()
        {
            return new DesEdeEngine();
        }
    }
}
