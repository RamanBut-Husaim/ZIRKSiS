namespace Kerberos.Services.Api.Contracts
{
    public interface ITgtToken
    {
        string ClientId { get; }
        byte[] IpAddress { get; }
        long TimeStamp { get; }
        long LifeStamp { get; }
        byte[] SessionKey { get; }
    }
}
