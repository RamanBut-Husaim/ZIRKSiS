using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Services.Api.Contracts.DataService;

namespace Kerberos.Services.Api.DataService
{
    public sealed class DataServiceRequest : IDataServiceRequest
    {
        public byte[] ServiceTicket { get; set; }
        public byte[] Authenticator { get; set; }
    }
}
