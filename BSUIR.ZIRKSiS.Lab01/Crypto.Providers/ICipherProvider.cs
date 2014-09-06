using Crypto.Providers.Ciphers;
using Raksha.Crypto;

namespace Crypto.Providers
{
    public interface ICipherProvider
    {
        IStreamCipher GetStreamCipher(StreamCiphers cipher);
        IBlockCipher GetBlockCipher(BlockCiphers cipher);
    }
}
