using System;

namespace Kerberos.Services.Api.Contracts
{
    public interface IAuthenticationRequest
    {
        string UserId { get; set; }
        DateTime TimeStamp { get; set; }
        string ServerId { get; set; }
    }
}
