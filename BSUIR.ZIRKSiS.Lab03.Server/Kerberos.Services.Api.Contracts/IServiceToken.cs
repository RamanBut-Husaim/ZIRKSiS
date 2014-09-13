namespace Kerberos.Services.Api.Contracts
{
    public interface IServiceToken
    {
        byte[] SessionKey { get; }
        string ServiceId { get; }
        long LifeTime { get; }
    }
}
