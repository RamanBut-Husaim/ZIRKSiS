using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Services.Api.Contracts.Authentication;

namespace Kerberos.Services.Api.Contracts.Tracing
{
    public interface ITraceManager : IDisposable
    {
        void TraceAuthRequest(string operationName, IAuthenticationRequest authenticationRequest);

        void TraceTgsAuthenticationReply(string operationName, ITgsToken tgsToken, ITgtToken tgtToken);

        void TraceTgsAuthenticationReply(string operationName, byte[] tgsToken, byte[] tgtToken);

        void TraceTgsToken(string operationName, ITgsToken tgsToken);

        void TraceByteArray(string operation, byte[] data);
    }
}
