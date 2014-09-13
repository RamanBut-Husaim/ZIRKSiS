using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Kerberos.Crypto.Contracts;

namespace Kerberos.Crypto
{
    internal sealed class HashAlgorithmProvider : IHashAlgorithmProvider
    {
        private readonly Lazy<IHashAlgorithmProvider> _instance = new Lazy<IHashAlgorithmProvider>(() => new HashAlgorithmProvider(), true);
        private readonly IDictionary<Hash, IHashAlgorithmBuilder> _hashAlgorithmContainer;

        public IHashAlgorithmProvider Instance
        {
            get { return _instance.Value; } 
        }

        private HashAlgorithmProvider()
        {
            _hashAlgorithmContainer = new Dictionary<Hash, IHashAlgorithmBuilder>()
            {
                {Hash.SHA1, new SHA1HashAlgorithmBuilder()},
                {Hash.SHA512, new SHA512HashAlgorithmBuilder()}
            };
        }

        public HashAlgorithm GetHashAlgorithm(Hash hashAlgorithm)
        {
            return _hashAlgorithmContainer[hashAlgorithm].Build();
        }
    }
}
