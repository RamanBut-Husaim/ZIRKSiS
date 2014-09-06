using System.Security.Cryptography;

namespace Crypto.Providers.BuildersStandard
{
    internal sealed class RSACipherBuilder : IAsymmetricCipherBuilder
    {
        private static readonly CspParameters CspParameters;

        static RSACipherBuilder()
        {
            CspParameters = new CspParameters
            {
                KeyContainerName = "Lab01Container"
            };
        }

        public AsymmetricAlgorithm Build()
        {
            return new RSACryptoServiceProvider(CspParameters)
            {
                PersistKeyInCsp = true
            };
        }
    }
}
