using System;
using System.Security.Cryptography;

namespace SKey.Service
{
    public sealed class SKeyHashManager : ISKeyHashManager
    {
        private const int DefaultIterationCount = 12;

        private readonly HashAlgorithm _hashAlgorithm;

        private byte[] _hashValue;
        private bool _disposed;

        private int _iterationCount;

        public SKeyHashManager(HashAlgorithm hashAlgorithm, string initialHash) : this(hashAlgorithm, initialHash, DefaultIterationCount)
        {
        }

        public SKeyHashManager(HashAlgorithm hashAlgorithm, string hashValueValue, int iterationCount)
        {
            this._iterationCount = iterationCount;
            this._hashAlgorithm = hashAlgorithm;
            this._hashValue = hashValueValue.ToByteArray();
        }

        public SKeyHashManager(HashAlgorithm hashAlgorithm, byte[] initialHash) : this(hashAlgorithm, initialHash, DefaultIterationCount)
        {
        }

        public SKeyHashManager(HashAlgorithm hashAlgorithm, byte[] initialHash, int iterationCount)
        {
            this._iterationCount = iterationCount;
            this._hashAlgorithm = hashAlgorithm;
            this._hashValue = initialHash.Clone() as byte[];
        }

        public int IterationCount
        {
            get
            {
                return this._iterationCount;
            }
        }

        public byte[] HashValue
        {
            get
            {
                return this._hashValue.Clone() as byte[];
            }
        }

        public bool VerifyHash(string hashValue)
        {
            return this.VerifyHash(hashValue.ToByteArray());
        }

        public bool VerifyHash(byte[] hashValue)
        {
            if (this._iterationCount <= 0)
            {
                throw new Exception("Iterations are completed");
            }

            bool result = this._hashAlgorithm.ComputeHash(hashValue).Compare(this._hashValue);
            if (result)
            {
                this._iterationCount--;
                this._hashValue = hashValue.Clone() as byte[];
            }

            return result;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._hashAlgorithm.Dispose();
                    this._disposed = true;
                }
            }
        }
    }
}
