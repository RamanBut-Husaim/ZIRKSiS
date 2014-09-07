using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoHash.Hashing
{
    internal sealed class HashingManager : IHashingManager
    {
        private bool _disposed;
        private readonly IPasswordProvider _passwordProvider;
        private readonly HashAlgorithm _hashAlgorithm;
        private const int BufferSize = 512;

        public HashingManager(HashAlgorithm hashAlgorithm, IPasswordProvider passwordProvider)
        {
            _passwordProvider = passwordProvider;
            _hashAlgorithm = hashAlgorithm;
        }

        public byte[] ComputeHash(string fileName, string outputFile, string password)
        {
            byte[] computedHash = this.ComputeHash(fileName, password);
            this.SaveHash(computedHash, outputFile);
            return computedHash;
        }

        public byte[] ComputeHash(string fileName, string password)
        {
            byte[] salt = _passwordProvider.GenerateSalt();

            using (var keyByteGenerator = new Rfc2898DeriveBytes(password, salt))
            {
                using (var binaryReader = new BinaryReader(File.OpenRead(fileName), new ASCIIEncoding()))
                {
                    bool stopReading;
                    do
                    {
                        byte[] buffer = binaryReader.ReadBytes(BufferSize);
                        stopReading = buffer.Length < BufferSize;
                        if (stopReading)
                        {
                            var initialLength = buffer.Length;
                            Array.Resize(ref buffer, buffer.Length + BufferSize);
                            byte[] randomBytes = keyByteGenerator.GetBytes(BufferSize);
                            Array.Copy(randomBytes, 0, buffer, initialLength, BufferSize);
                            _hashAlgorithm.TransformFinalBlock(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            var temp = new byte[buffer.Length];
                            _hashAlgorithm.TransformBlock(buffer, 0, buffer.Length, temp, 0);
                        }
                    } while (!stopReading);
                }
            }

            return _hashAlgorithm.Hash;
        }


        public void Dispose()
        {
            this.Dispose(true);
        }

        private void SaveHash(byte[] hash, string fileName)
        {
            using (var fileStream = File.Create(fileName))
            {
                fileStream.Write(hash, 0, hash.Length);
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _passwordProvider.Dispose();
                    _disposed = true;
                }
            }
        }
    }
}
