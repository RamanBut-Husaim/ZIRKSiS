using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Crypto.Providers.BuildersStandard;
using Crypto.Providers.Ciphers;

namespace Crypto.Providers
{
    internal sealed class CipherProviderStandard : ICipherProviderStandard
    {
        private static readonly Dictionary<SymmetricCiphers, ISymmetricCipherBuilder> SymmetricCipherBuilders;

        static CipherProviderStandard()
        {
            SymmetricCipherBuilders = new Dictionary<SymmetricCiphers, ISymmetricCipherBuilder>()
            {
                { SymmetricCiphers.AES, new AESCipherBuilder() },
                { SymmetricCiphers.DES, new DESCipherBuilder() },
                { SymmetricCiphers.RC2, new RC2CipherBuilder() },
                { SymmetricCiphers.Rijndael, new RijndaelCipherBuilder() },
                { SymmetricCiphers.TripleDES, new TripleDESCipherBuilder() } 
            };
        }


        public SymmetricAlgorithm GetSymmetricCipher(SymmetricCiphers cipher)
        {
            return SymmetricCipherBuilders[cipher].Build();
        }
    }
}
