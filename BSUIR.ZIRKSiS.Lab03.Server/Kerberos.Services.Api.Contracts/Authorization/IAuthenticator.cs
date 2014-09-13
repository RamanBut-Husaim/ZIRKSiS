using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerberos.Services.Api.Contracts.Authorization
{
    public interface IAuthenticator
    {
        string ClientId { get; }
        long TimeStamp { get; }
    }
}
