using System.Collections.Generic;
using System.Security.Cryptography;
using Crypto.Providers.BuildersStandard;
using Crypto.Providers.Ciphers;

namespace Crypto.Providers
{
    internal sealed class CipherProviderStandard : ICipherProviderStandard
    {
        private static readonly Dictionary<SymmetricCipher, ISymmetricCipherBuilder> SymmetricCipherBuilders;
        private static readonly Dictionary<AsymmetricCipher, IAsymmetricCipherBuilder> AsymmetricCipherBuilders; 

        static CipherProviderStandard()
        {
            SymmetricCipherBuilders = new Dictionary<SymmetricCipher, ISymmetricCipherBuilder>()
            {
                { SymmetricCipher.AES, new AESCipherBuilder() },
                { SymmetricCipher.DES, new DESCipherBuilder() },
                { SymmetricCipher.RC2, new RC2CipherBuilder() },
                { SymmetricCipher.Rijndael, new RijndaelCipherBuilder() },
                { SymmetricCipher.TripleDES, new TripleDESCipherBuilder() } 
            };

            AsymmetricCipherBuilders = new Dictionary<AsymmetricCipher, IAsymmetricCipherBuilder>()
            {
                { AsymmetricCipher.RSA, new RSACipherBuilder() }
            };
        }


        public SymmetricAlgorithm GetSymmetricCipher(SymmetricCipher cipher)
        {
            return SymmetricCipherBuilders[cipher].Build();
        }

        public AsymmetricAlgorithm GetAsymmetricCipher(AsymmetricCipher cipher)
        {
            return AsymmetricCipherBuilders[cipher].Build();
        }
    }
}
