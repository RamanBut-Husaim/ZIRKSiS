namespace CryptoHash.Providers
{
    public sealed class HashAlgorithmProviderBuilder : IHashAlgorithmProviderBuilder
    {
        public IHashAlgorithmProvider Build()
        {
            return new HashAlgorithmProvider();
        }
    }
}
