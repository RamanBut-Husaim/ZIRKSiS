using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Services.Api.Contracts.DataService;

namespace Kerberos.Services.Api.DataService
{
    internal sealed class DataServiceReply : IDataServiceReply
    {
        public byte[] TimeStamp { get; internal set; }
    }
}
