using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Crypto.Contracts;

namespace Kerberos.Crypto
{
    [Serializable]
    internal sealed class TgsToken : ITgsToken
    {
        public TgsToken()
        {
        }

        public byte[] SessionKey { get; private set; }
        public string Id { get; private set; }
        public long LifeTime { get; private set; }
    }
}
