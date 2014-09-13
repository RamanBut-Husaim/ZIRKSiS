using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Services.Api.Contracts.Authentication;

namespace Kerberos.Services.Api.Contracts.Authorization
{
    public interface IAuthorizationService : IService
    {
        byte[] CreateAuthenticator(string userId, byte[] sessionKey);

        IAuthorizationReply Authorize(IAuthorizationRequest authorizationRequest);
    }
}
