namespace Kerberos.Services.Api.Contracts
{
    public interface ITgsToken
    {
        byte[] SessionKey { get; }
        string Id { get; }
        long LifeTime { get; }
    }
}
