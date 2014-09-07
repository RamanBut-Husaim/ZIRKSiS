using System;

namespace CryptoHash.Hashing
{
    public interface IHashingManager: IDisposable
    {
        byte[] ComputeHash(string fileName, string password);
        byte[] ComputeHash(string fileName, string outputFile, string password);
    }
}
