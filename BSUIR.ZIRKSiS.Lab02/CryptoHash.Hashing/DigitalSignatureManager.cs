using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace CryptoHash.Hashing
{
    public sealed class DigitalSignatureManager : IDigitalSignatureManager
    {
        private bool _disposed;
        private readonly RSACryptoServiceProvider _rsaCryptoServiceProvider;

        internal DigitalSignatureManager()
        {
            _rsaCryptoServiceProvider = new RSACryptoServiceProvider();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void CreateSignature(string inputFile, string signatureFile, string keyFile)
        {
            RSAParameters rsaPublicParameters = _rsaCryptoServiceProvider.ExportParameters(false);
            using (var inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                byte[] signature = _rsaCryptoServiceProvider.SignData(inputStream, CryptoConfig.MapNameToOID("SHA1"));
                this.SaveSignature(signature, signatureFile);
                this.SavePublicKey(rsaPublicParameters, keyFile);
            }
        }

        public bool VerifySignature(string inputFile, string signatureFile, string keyFile)
        {
            bool result;
            RSAParameters rsaPublicParameters = this.RestorePublicKey(keyFile);
            _rsaCryptoServiceProvider.ImportParameters(rsaPublicParameters);
            using (var inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                byte[] signature = this.ReadSignature(signatureFile);
                var buffer = new byte[inputStream.Length];
                inputStream.Read(buffer, 0, buffer.Length);
                result = _rsaCryptoServiceProvider.VerifyData(buffer, CryptoConfig.MapNameToOID("SHA1"), signature);
            }

            return result;
        }

        private void SaveSignature(byte[] signature, string signatureFile)
        {
            using (var file = File.Create(signatureFile))
            {
                file.Write(signature, 0, signature.Length);
            }
        }

        private byte[] ReadSignature(string signatureFile)
        {
            byte[] result;

            using (var file = new FileStream(signatureFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                result = new byte[file.Length];
                file.Read(result, 0, result.Length);
            }

            return result;
        }

        private void SavePublicKey(RSAParameters rsaParameters, string keyFile)
        {
            using (var file = new FileStream(keyFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(file, rsaParameters);
            }
        }

        private RSAParameters RestorePublicKey(string keyFile)
        {
            RSAParameters? result;
            using (var file = new FileStream(keyFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var binaryFormatter = new BinaryFormatter();
                result = binaryFormatter.Deserialize(file) as RSAParameters?;
            }

            return result.Value;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _rsaCryptoServiceProvider.Dispose();
                    _disposed = true;
                }
            }
        }

        public static IDigitalSignatureManager Create()
        {
            return new DigitalSignatureManager();
        }
    }
}
