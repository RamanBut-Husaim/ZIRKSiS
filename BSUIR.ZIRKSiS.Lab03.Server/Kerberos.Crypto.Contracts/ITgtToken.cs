using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerberos.Crypto.Contracts
{
    public interface ITgtToken
    {
        string ClientId { get; }
        byte[] IpAddress { get; }
        long TimeStamp { get; set; }
        long LifeStamp { get; set; }
        byte[] SessionKey { get; }
    }
}
