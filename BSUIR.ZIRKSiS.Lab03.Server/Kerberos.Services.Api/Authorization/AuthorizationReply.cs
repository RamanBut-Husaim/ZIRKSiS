using Kerberos.Services.Api.Contracts.Authorization;

namespace Kerberos.Services.Api.Authorization
{
    public sealed class AuthorizationReply : IAuthorizationReply
    {
        public byte[] ServiceTicket { get; internal set; }
        public byte[] ServiceToken { get; internal set; }
    }
}
