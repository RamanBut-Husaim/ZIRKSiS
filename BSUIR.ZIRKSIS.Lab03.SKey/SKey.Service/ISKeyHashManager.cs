using System;

namespace SKey.Service
{
    public interface ISKeyHashManager : IDisposable
    {
        bool VerifyHash(string hashValue);

        bool VerifyHash(byte[] hashValue);
    }
}
