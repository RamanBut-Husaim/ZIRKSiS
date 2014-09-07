using System;
using System.IO;
using System.Security.Cryptography;

namespace CryptoHash.Hashing
{
    internal sealed class SecureHashingManager : IHashingManager
    {
        private bool _disposed;
        private readonly HMAC _secureHashAlgorithm;
        private readonly IPasswordProvider _passwordProvider;

        public SecureHashingManager(HMAC secureHashAlgorithm, IPasswordProvider passwordProvider)
        {
            _passwordProvider = passwordProvider;
            _secureHashAlgorithm = secureHashAlgorithm;
        }

        public byte[] ComputeHash(string fileName, string password)
        {
            byte[] passwordBytes = this.GetBytes(password);
            _secureHashAlgorithm.Key = passwordBytes;
            byte[] result = this._secureHashAlgorithm.ComputeHash(File.OpenRead(fileName));

            return result;
        }

        public byte[] ComputeHash(string fileName, string outputFile, string password)
        {
            byte[] computedHash = this.ComputeHash(fileName, password);
            this.SaveHash(computedHash, outputFile);

            return computedHash;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _secureHashAlgorithm.Dispose();
                    _passwordProvider.Dispose();
                    _disposed = true;
                }
            }
        }

        private void SaveHash(byte[] hash, string fileName)
        {
            using (var fileStream = File.Create(fileName))
            {
                fileStream.Write(hash, 0, hash.Length);
            }
        }

        private byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
