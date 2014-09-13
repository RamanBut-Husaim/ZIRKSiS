namespace Kerberos.Services.Api.Contracts.Authentication
{
    public interface IAuthenticationService : IService
    {
        IAuthenticationReply Authenticate(IAuthenticationRequest authenticationRequest);

        ITgsToken DecryptReply(string userId, IAuthenticationReply authenticationReply);
    }
}
