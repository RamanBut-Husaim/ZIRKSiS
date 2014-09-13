using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerberos.Services.Api.Contracts.DataService
{
    public interface IDataService : IService
    {
        byte[] CreateAuthenticator(string userId, byte[] sessionKey);

        IDataServiceReply GetAccess(IDataServiceRequest dataServiceRequest);

        ITimeStampContainer DecryptReply(string userId, IDataServiceReply dataServiceReply, byte[] sessionKey);
    }
}
