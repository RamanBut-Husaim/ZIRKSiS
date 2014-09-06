using Raksha.Crypto;
using Raksha.Crypto.Engines;

namespace Crypto.Providers.Builders
{
    internal sealed class DESBlockCipherBuilder: IBlockCipherBuilder
    {
        public IBlockCipher Build()
        {
            return new DesEngine();
        }
    }
}
