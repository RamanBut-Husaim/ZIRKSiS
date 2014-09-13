using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Crypto.Contracts;

namespace Kerberos.Crypto
{
    internal sealed class AESAlgorithmBuilder : ISymmetricAlgorithmBuilder
    {
        public SymmetricAlgorithm Build()
        {
            return new AesCryptoServiceProvider();
        }
    }
}
