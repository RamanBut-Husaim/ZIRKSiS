using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerberos.Services.Api.Contracts
{
    public interface ITimeStampContainer
    {
        long TimeStamp { get; }
    }
}
