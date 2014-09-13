using Kerberos.Services.Api.Contracts.Authorization;

namespace Kerberos.Services.Api.Authorization
{
    public sealed class AuthorizationRequest : IAuthorizationRequest
    {
        public byte[] TgtBytes { get; set; }

        public byte[] AutheticatorBytes { get; set; }
    }
}
