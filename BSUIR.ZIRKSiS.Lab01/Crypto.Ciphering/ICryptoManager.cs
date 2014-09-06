using System;

namespace Crypto.Ciphering
{
    public interface ICryptoManager : IDisposable
    {
        string CryptoAlgorithm { get; }
        string HashAlogithm { get; }
        void Encrypt(string inputFile, string outputFile, string password);
        void Encrypt(string inputFile, string outputFile);
        void Decrypt(string inputFile, string outputFile, string password);
        void Decrypt(string inputFile, string outputFile);
    }
}
