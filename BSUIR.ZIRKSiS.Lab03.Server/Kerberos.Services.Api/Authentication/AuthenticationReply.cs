using Kerberos.Services.Api.Contracts.Authentication;

namespace Kerberos.Services.Api.Authentication
{
    public sealed class AuthenticationReply : IAuthenticationReply
    {
        public byte[] TgsBytes { get; internal set; }
        public byte[] TgtBytes { get; internal set; }
        public string Message { get; internal set; }
    }
}
