using Raksha.Crypto;
using Raksha.Crypto.Engines;

namespace Crypto.Providers.Builders
{
    internal sealed class RC4StreamCipherBuilder : IStreamCipherBuilder
    {
        public IStreamCipher Build()
        {
            return new RC4Engine();
        }
    }
}
