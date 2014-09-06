using System.Collections.Generic;
using Crypto.Providers.Builders;
using Crypto.Providers.Ciphers;
using Raksha.Crypto;

namespace Crypto.Providers
{
    internal sealed class CipherProvider : ICipherProvider
    {
        private static readonly Dictionary<BlockCipher, IBlockCipherBuilder> BlockCipherBuilders;
        private static readonly Dictionary<StreamCipher, IStreamCipherBuilder> StreamCipherBuilders;

        static CipherProvider()
        {
            BlockCipherBuilders = new Dictionary<BlockCipher, IBlockCipherBuilder>(4)
            {
                { BlockCipher.RC2, new RC2BlockCipherBuilder() },
                { BlockCipher.DES, new DESBlockCipherBuilder() },
                { BlockCipher.DESede, new DESedeBlockCipherBuilder() },
                { BlockCipher.AES, new AESBlockCipherBuilder() }
            };
            StreamCipherBuilders = new Dictionary<StreamCipher, IStreamCipherBuilder>()
            {
                { StreamCipher.RC4, new RC4StreamCipherBuilder() }
            };
        }


        public IStreamCipher GetStreamCipher(StreamCipher cipher)
        {
            return StreamCipherBuilders[cipher].Build();
        }

        public IBlockCipher GetBlockCipher(BlockCipher cipher)
        {
            return BlockCipherBuilders[cipher].Build();
        }
    }
}
