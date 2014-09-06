using Crypto.Providers.Ciphers;
using Raksha.Crypto;

namespace Crypto.Providers
{
    public interface ICipherProvider
    {
        IStreamCipher GetStreamCipher(StreamCipher cipher);
        IBlockCipher GetBlockCipher(BlockCipher cipher);
    }
}
