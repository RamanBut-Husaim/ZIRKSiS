using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Crypto.Contracts;

namespace Kerberos.Crypto
{
    internal sealed class TgtToken : ITgtToken
    {
        public TgtToken()
        {
            
        }

        public string ClientId { get; private set; }
        public byte[] IpAddress { get; private set; }
        public long TimeStamp { get; set; }
        public long LifeStamp { get; set; }
        public byte[] SessionKey { get; private set; }
    }
}
