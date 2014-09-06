using System.Collections.Generic;
using Crypto.Providers.Builders;
using Crypto.Providers.Ciphers;
using Raksha.Crypto;

namespace Crypto.Providers
{
    internal sealed class CipherProvider : ICipherProvider
    {
        private static readonly Dictionary<BlockCiphers, IBlockCipherBuilder> BlockCipherBuilders;
        private static readonly Dictionary<StreamCiphers, IStreamCipherBuilder> StreamCipherBuilders;

        static CipherProvider()
        {
            BlockCipherBuilders = new Dictionary<BlockCiphers, IBlockCipherBuilder>(4)
            {
                { BlockCiphers.RC2, new RC2BlockCipherBuilder() },
                { BlockCiphers.DES, new DESBlockCipherBuilder() },
                { BlockCiphers.DESede, new DESedeBlockCipherBuilder() },
                { BlockCiphers.AES, new AESBlockCipherBuilder() }
            };
            StreamCipherBuilders = new Dictionary<StreamCiphers, IStreamCipherBuilder>()
            {
                { StreamCiphers.RC4, new RC4StreamCipherBuilder() }
            };
        }


        public IStreamCipher GetStreamCipher(StreamCiphers cipher)
        {
            return StreamCipherBuilders[cipher].Build();
        }

        public IBlockCipher GetBlockCipher(BlockCiphers cipher)
        {
            return BlockCipherBuilders[cipher].Build();
        }
    }
}
