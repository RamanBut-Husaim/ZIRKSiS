using System.Security.Cryptography;
using Kerberos.Crypto.Contracts;

namespace Kerberos.Crypto
{
    internal sealed class TripleDESAlgorithmBuilder : ISymmetricAlgorithmBuilder
    {
        public SymmetricAlgorithm Build()
        {
            return new TripleDESCryptoServiceProvider();
        }
    }
}
