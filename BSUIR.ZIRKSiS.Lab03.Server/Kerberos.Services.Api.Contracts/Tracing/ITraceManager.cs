using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Services.Api.Contracts.Authentication;
using Kerberos.Services.Api.Contracts.Authorization;

namespace Kerberos.Services.Api.Contracts.Tracing
{
    public interface ITraceManager : IDisposable
    {
        void Trace<T>(string operationName, T obj) where T : class;

        void Trace<T1, T2>(string operationName, T1 obj1, T2 obj2) where T1 : class where T2 : class;
    }
}
