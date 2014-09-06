namespace Crypto.Providers
{
    public sealed class HashAlgorithmProviderStandardBuilder : IHashAlgorithmProviderStandardBuilder
    {
        public IHashAlgorithmProviderStandard Build()
        {
            return new HashAlgorithmProviderStandard();
        }
    }
}