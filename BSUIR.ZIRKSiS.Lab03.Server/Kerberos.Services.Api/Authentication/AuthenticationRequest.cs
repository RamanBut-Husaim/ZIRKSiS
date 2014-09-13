using System;
using Kerberos.Services.Api.Contracts;
using Kerberos.Services.Api.Contracts.Authentication;

namespace Kerberos.Services.Api.Authentication
{
    public sealed class AuthenticationRequest : IAuthenticationRequest
    {
        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string ServerId { get; set; }
    }
}
