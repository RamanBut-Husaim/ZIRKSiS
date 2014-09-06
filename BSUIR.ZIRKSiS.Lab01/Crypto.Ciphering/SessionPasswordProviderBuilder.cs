using System.Security.Cryptography;

namespace Crypto.Ciphering
{
    public sealed class SessionPasswordProviderBuilder : IPasswordProviderBuilder
    {
        private readonly AsymmetricAlgorithm _asymmetricAlgorithm;

        public SessionPasswordProviderBuilder(AsymmetricAlgorithm asymmetricAlgorithm)
        {
            _asymmetricAlgorithm = asymmetricAlgorithm;
        }

        public IPasswordProvider Build()
        {
            return new SessionPasswordProvider(_asymmetricAlgorithm);
        }
    }
}
