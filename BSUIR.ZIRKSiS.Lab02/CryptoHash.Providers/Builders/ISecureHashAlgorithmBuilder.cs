using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace CryptoHash.Providers.Builders
{
    public interface ISecureHashAlgorithmBuilder
    {
        HMAC Build();
    }
}
