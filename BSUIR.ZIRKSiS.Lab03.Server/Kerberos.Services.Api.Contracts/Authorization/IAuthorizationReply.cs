namespace Kerberos.Services.Api.Contracts.Authorization
{
    public interface IAuthorizationReply
    {
        byte[] ServiceTicket { get; }
        byte[] ServiceToken { get; }
    }
}
