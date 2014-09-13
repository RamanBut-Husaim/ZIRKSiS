using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Crypto.Contracts;

namespace Kerberos.Crypto
{
    internal sealed class SHA1HashAlgorithmBuilder : IHashAlgorithmBuilder
    {
        public HashAlgorithm Build()
        {
            return new SHA1CryptoServiceProvider();
        }
    }
}
