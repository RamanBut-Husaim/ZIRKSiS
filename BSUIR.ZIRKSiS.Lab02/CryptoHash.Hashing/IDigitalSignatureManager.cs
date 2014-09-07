using System;

namespace CryptoHash.Hashing
{
    public interface IDigitalSignatureManager : IDisposable
    {
        void CreateSignature(string inputFile, string signatureFile, string keyFile);
        bool VerifySignature(string inputFile, string signatureFile, string keyFile);
    }
}
