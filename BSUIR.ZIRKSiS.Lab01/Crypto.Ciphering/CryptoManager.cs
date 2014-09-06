using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Crypto.Ciphering
{
    public sealed class CryptoManager : ICryptoManager
    {
        private readonly SymmetricAlgorithm _symmetricAlgorithm;
        private readonly HashAlgorithm _hashAlgorithm;
        private readonly IPasswordProvider _passwordProvider;
        private bool _disposed;

        public string CryptoAlgorithm { get { return "Algorithm"; } }
        public string HashAlogithm { get { return "Hash Algorithm"; } }                                                

        internal CryptoManager(
            SymmetricAlgorithm symmetricAlgorithm, 
            HashAlgorithm hashAlgorithm,
            IPasswordProvider passwordProvider)
        {
            _symmetricAlgorithm = symmetricAlgorithm;
            _hashAlgorithm = hashAlgorithm;
            _passwordProvider = passwordProvider;
        }

        public static ICryptoManager Create(SymmetricAlgorithm symmetricAlgorithm, HashAlgorithm hashAlgorithm, IPasswordProvider passwordProvider)
        {
            return new CryptoManager(symmetricAlgorithm, hashAlgorithm, passwordProvider);
        }

        public void Encrypt(string inputFile, string outputFile, string password)
        {
            byte[] keyBytes = this.GetKey(password);
            byte[] ivBytes = this.GetIV();

            var encryptor = _symmetricAlgorithm.CreateEncryptor(keyBytes, ivBytes);
            int blockSize = _symmetricAlgorithm.BlockSize/8;

            using (var binaryReader = new BinaryReader(File.OpenRead(inputFile), new ASCIIEncoding()))
            {
                using (var resultFile = File.Create(outputFile))
                {
                    using (var cryptoStream = new CryptoStream(resultFile, encryptor, CryptoStreamMode.Write))
                    {
                        using (var binaryWriter = new BinaryWriter(cryptoStream))
                        {
                            while (binaryReader.PeekChar() != -1)
                            {
                                binaryWriter.Write(binaryReader.ReadBytes(blockSize));
                            }
                        }
                    }
                }
            }
        }

        public void Encrypt(string inputFile, string outputFile)
        {
            byte[] passwordBytes = _passwordProvider.GetPassword();
            var password = this.GetString(passwordBytes);
            this.Encrypt(inputFile, outputFile, password);
        }

        public void Decrypt(string inputFile, string outputFile, string password)
        {
            byte[] byteKeys = this.GetKey(password);
            byte[] ivBytes = this.GetIV();

            var decryptor = _symmetricAlgorithm.CreateDecryptor(byteKeys, ivBytes);
            int blockSize = _symmetricAlgorithm.BlockSize / 8;

            using (var binaryReader = new BinaryReader(File.OpenRead(inputFile), new ASCIIEncoding()))
            {
                using (var resultFile = File.Create(outputFile))
                {
                    using (var cryptoStream = new CryptoStream(resultFile, decryptor, CryptoStreamMode.Write))
                    {
                        using (var binaryWriter = new BinaryWriter(cryptoStream))
                        {
                            while (binaryReader.PeekChar() != -1)
                            {
                                binaryWriter.Write(binaryReader.ReadBytes(blockSize));
                            }
                        }
                    }
                }
            }
        }

        public void Decrypt(string inputFile, string outputFile)
        {
            byte[] passwordBytes = _passwordProvider.GetPassword();
            var password = this.GetString(passwordBytes);
            this.Decrypt(inputFile, outputFile, password);
        }

        private byte[] GetKey(string password)
        {
            byte[] passwordHash = this.GetHash(password);
            var keyBytes = new byte[_symmetricAlgorithm.LegalKeySizes[0].MaxSize / 8];
            Buffer.BlockCopy(passwordHash, 0, keyBytes, 0, keyBytes.Length);
            return keyBytes;
        }

        private string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private byte[] GetIV()
        {
            byte[] nameHash = this.GetHash(Environment.UserName);
            var ivBytes = new byte[_symmetricAlgorithm.LegalBlockSizes[0].MinSize / 8];
            Buffer.BlockCopy(nameHash, 0, ivBytes, 0, ivBytes.Length);
            return ivBytes;
        }

        private byte[] GetHash(string data)
        {
            return _hashAlgorithm.ComputeHash(GetBytes(data));
        }

        private byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
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
                    _symmetricAlgorithm.Dispose();
                    _hashAlgorithm.Dispose();
                    _passwordProvider.Dispose();
                    _disposed = true;
                }
            }
        }
    }
}
