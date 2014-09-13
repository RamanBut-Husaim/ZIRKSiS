using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Kerberos.Crypto.Contracts
{
    public interface ITgsToken
    {
        byte[] SessionKey { get; }
        string Id { get; }
        long LifeTime { get; }
    }
}
