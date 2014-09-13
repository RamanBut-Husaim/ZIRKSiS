using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Crypto.Contracts;

namespace Kerberos.Crypto
{
    internal sealed class SymmetricAlgorithmProvider : ISymmetricAlgorithmProvider
    {
        private readonly Lazy<ISymmetricAlgorithmProvider> _instance = new Lazy<ISymmetricAlgorithmProvider>(() => new SymmetricAlgorithmProvider(), true);
        private readonly IDictionary<SymmetricAlgorithms, ISymmetricAlgorithmBuilder> _symmetricAlgorithmContainer; 

        private SymmetricAlgorithmProvider()
        {
            _symmetricAlgorithmContainer = new Dictionary<SymmetricAlgorithms, ISymmetricAlgorithmBuilder>()
            {
                {SymmetricAlgorithms.AES, new AESAlgorithmBuilder()},
                {SymmetricAlgorithms.TripleDES, new TripleDESAlgorithmBuilder()}
            };
        }

        public SymmetricAlgorithm GetSymmetricAlgorithm(SymmetricAlgorithms symmetricAlgorithm)
        {
            return _symmetricAlgorithmContainer[symmetricAlgorithm].Build();
        }
    }
}
