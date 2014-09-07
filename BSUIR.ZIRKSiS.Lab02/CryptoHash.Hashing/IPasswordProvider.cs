using System;

namespace CryptoHash.Hashing
{
    public interface IPasswordProvider : IDisposable
    {
        byte[] GetPassword();
        byte[] GenerateSalt();
    }
}
