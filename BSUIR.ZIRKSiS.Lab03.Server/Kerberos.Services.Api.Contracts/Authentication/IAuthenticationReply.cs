namespace Kerberos.Services.Api.Contracts.Authentication
{
    public interface IAuthenticationReply
    {
        byte[] TgsBytes { get; }
        byte[] TgtBytes { get; }
        string Message { get; }
    }
}
