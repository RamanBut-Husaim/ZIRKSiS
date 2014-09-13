namespace Kerberos.Services.Api.Contracts.Authorization
{
    public interface IAuthorizationRequest
    {
        byte[] TgtBytes { get; set; }
        byte[] AutheticatorBytes { get; set; }
    }
}
