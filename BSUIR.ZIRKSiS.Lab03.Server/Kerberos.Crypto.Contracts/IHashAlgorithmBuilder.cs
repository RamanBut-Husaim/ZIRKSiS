﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kerberos.Crypto.Contracts
{
    public interface IHashAlgorithmBuilder
    {
        HashAlgorithm Build();
    }
}
