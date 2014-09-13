namespace Kerberos.Services.Api.Contracts.Authorization
{
    public interface IAuthorizationService : IService
    {
        byte[] CreateAuthenticator(string userId, byte[] sessionKey);

        IAuthorizationReply Authorize(IAuthorizationRequest authorizationRequest);

        IServiceToken DecryptReply(string userId, IAuthorizationReply reply, byte[] sessionKey);
    }
}
