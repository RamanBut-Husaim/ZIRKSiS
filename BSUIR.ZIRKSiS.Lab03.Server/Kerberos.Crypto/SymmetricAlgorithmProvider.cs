using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Kerberos.Crypto.Contracts;

namespace Kerberos.Crypto
{
    public sealed class SymmetricAlgorithmProvider : ISymmetricAlgorithmProvider
    {
        private readonly static Lazy<ISymmetricAlgorithmProvider> _instance = new Lazy<ISymmetricAlgorithmProvider>(() => new SymmetricAlgorithmProvider(), true);
        private readonly IDictionary<SymmetricAlgorithms, ISymmetricAlgorithmBuilder> _symmetricAlgorithmContainer;

        public static ISymmetricAlgorithmProvider Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private SymmetricAlgorithmProvider()
        {
            this._symmetricAlgorithmContainer = new Dictionary<SymmetricAlgorithms, ISymmetricAlgorithmBuilder>()
            {
                { SymmetricAlgorithms.AES, new AESAlgorithmBuilder() },
                { SymmetricAlgorithms.TripleDES, new TripleDESAlgorithmBuilder() }
            };
        }

        public SymmetricAlgorithm GetSymmetricAlgorithm(SymmetricAlgorithms symmetricAlgorithm)
        {
            return this._symmetricAlgorithmContainer[symmetricAlgorithm].Build();
        }
    }
}
