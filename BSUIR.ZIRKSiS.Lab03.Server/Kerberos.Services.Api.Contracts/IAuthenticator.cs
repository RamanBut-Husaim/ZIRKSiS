namespace Kerberos.Services.Api.Contracts
{
    public interface IAuthenticator
    {
        string ClientId { get; }
        long TimeStamp { get; }
    }
}
