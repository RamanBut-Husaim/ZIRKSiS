using System.Security.Cryptography;
using Crypto.Providers.Ciphers;

namespace Crypto.Providers
{
    public interface ICipherProviderStandard
    {
        SymmetricAlgorithm GetSymmetricCipher(SymmetricCipher cipher);
        AsymmetricAlgorithm GetAsymmetricCipher(AsymmetricCipher cipher);
    }
}
