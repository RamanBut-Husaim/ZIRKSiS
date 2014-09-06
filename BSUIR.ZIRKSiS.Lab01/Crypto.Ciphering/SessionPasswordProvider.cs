using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Crypto.Ciphering
{
    internal sealed class SessionPasswordProvider :IPasswordProvider
    {
        private static readonly string KeyName = "key.txt";
        private static readonly int PasswordLength = 20;

        private readonly RSACryptoServiceProvider _asymmetricAlgorithm;
        private readonly RNGCryptoServiceProvider _rngCryptoServiceProvider;
        private bool _disposed;

        public SessionPasswordProvider(AsymmetricAlgorithm asymmetricAlgorithm)
        {
            _asymmetricAlgorithm = asymmetricAlgorithm as RSACryptoServiceProvider;
            _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        public byte[] GetPassword()
        {
            byte[] result;

            try
            {
                using (var binaryReader = new BinaryReader(File.OpenRead(KeyName),new ASCIIEncoding()))
                {
                    var buffer = new byte[128];
                    binaryReader.Read(buffer, 0, buffer.Length);
                    result = _asymmetricAlgorithm.Decrypt(buffer, true);
                }
            }
            catch (FileNotFoundException)
            {
                result = new byte[PasswordLength];
                _rngCryptoServiceProvider.GetBytes(result);
                this.SavePassword(result);
            }

            return result;
        }

        private void SavePassword(byte[] passwordBytes)
        {
            byte[] encryptedPassword = _asymmetricAlgorithm.Encrypt(passwordBytes, true);
            using (var binaryWriter = new BinaryWriter(File.Create(KeyName), new ASCIIEncoding()))
            {
                binaryWriter.Write(encryptedPassword);
            }
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
            if (_disposed)
            {
                if (disposing)
                {
                    _asymmetricAlgorithm.Dispose();
                    _rngCryptoServiceProvider.Dispose();
                    _disposed = true;
                }
            }
        }
    }
}
