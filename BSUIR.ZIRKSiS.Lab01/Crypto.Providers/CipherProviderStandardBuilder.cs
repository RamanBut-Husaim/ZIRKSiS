namespace Crypto.Providers
{
    public sealed class CipherProviderStandardBuilder : ICipherProviderStandardBuilder
    {
        public ICipherProviderStandard Build()
        {
            return new CipherProviderStandard();
        }
    }
}