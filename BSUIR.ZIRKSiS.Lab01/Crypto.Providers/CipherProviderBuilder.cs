namespace Crypto.Providers
{
    public sealed class CipherProviderBuilder : ICipherProviderBuilder
    {
        public ICipherProvider Build()
        {
            return new CipherProvider();
        }
    }
}
