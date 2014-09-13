using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Kerberos.Crypto.Contracts;
using Kerberos.Data.Contracts;

namespace Kerberos.Services.Api.Contracts
{
    public abstract class SecurityServiceBase : ServiceBase, IDisposable
    {
        protected const int SessionKeyLength = 32;

        protected const int BufferSize = 300;

        private const string TgsPasswordConst = "TgsPassword";

        private const string TgsSaltConst = "TgsSalt";

        private const string ServicePasswordConst = "ServicePassword";

        private const string ServiceSaltConst = "ServiceSalt";

        private const int IterationCount = 3;

        private readonly SymmetricAlgorithm _symmetricAlgorithm;

        private readonly RNGCryptoServiceProvider _rngCryptoServiceProvider;

        private bool _disposed;

        private byte[] _tgsKey;

        private byte[] _tgsIV;

        private byte[] _serviceKey;

        private byte[] _serviceIV;

        protected SecurityServiceBase(
            IUnitOfWork unitOfWork, 
            ISymmetricAlgorithmProvider symmetricAlgorithmProvider)
            : base(unitOfWork)
        {
            this._rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            this._symmetricAlgorithm = symmetricAlgorithmProvider.GetSymmetricAlgorithm(SymmetricAlgorithms.AES);
        }

        protected SymmetricAlgorithm SymmetricAlgorithm
        {
            get
            {
                return this._symmetricAlgorithm;
            }
        }

        protected byte[] TgsKey
        {
            get
            {
                if (this._tgsKey == null)
                {
                    this._tgsKey = this.GenerateKey(TgsPasswordConst, TgsSaltConst);
                }

                return this._tgsKey.Clone() as byte[];
            }
        }

        protected byte[] TgsIV
        {
            get
            {
                if (this._tgsIV == null)
                {
                    this._tgsIV = this.GenerateIV(TgsPasswordConst, TgsSaltConst);
                }

                return this._tgsIV.Clone() as byte[];
            }
        }

        protected byte[] ServiceKey
        {
            get
            {
                if (this._serviceKey == null)
                {
                    this._serviceKey = this.GenerateKey(ServicePasswordConst, ServiceSaltConst);
                }

                return this._serviceKey.Clone() as byte[];
            }
        }

        protected byte[] ServiceIV
        {
            get
            {
                if (this._serviceIV == null)
                {
                    this._serviceIV = this.GenerateIV(TgsPasswordConst, TgsSaltConst);
                }

                return this._serviceIV.Clone() as byte[];
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected void GetRandomBytes(byte[] buffer)
        {
            this._rngCryptoServiceProvider.GetNonZeroBytes(buffer);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._rngCryptoServiceProvider.Dispose();
                    this._disposed = true;
                }
            }
        }

        protected byte[] GenerateKey(string password, string salt)
        {
            byte[] result; 
            using (var bytes = new Rfc2898DeriveBytes(password, salt.ToByteArray()))
            {
                result = bytes.GetBytes(this._symmetricAlgorithm.LegalKeySizes[0].MaxSize / 8);
            }

            return result;
        }

        protected byte[] GenerateIV(string password, string salt)
        {
            byte[] result;

            using (var bytes = new Rfc2898DeriveBytes(password, salt.ToByteArray(), IterationCount))
            {
                result = bytes.GetBytes(this._symmetricAlgorithm.LegalBlockSizes[0].MaxSize / 8);
            }

            return result;
        }

        protected byte[] Encrypt<T>(byte[] key, byte[] iv, T obj) where T : class 
        {
            byte[] serializedObject;
            byte[] result;

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, obj);
                serializedObject = memoryStream.ToArray();
            }

            using (var encryptor = this.SymmetricAlgorithm.CreateEncryptor(key, iv))
            {
                using (var targetStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(targetStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new BinaryWriter(cryptoStream))
                        {
                            writer.Write(serializedObject);
                            cryptoStream.FlushFinalBlock();
                            result = targetStream.ToArray();
                        }
                    }
                }
            }

            return result;
        }

        protected T Decrypt<T>(byte[] key, byte[] iv, byte[] obj) where T : class 
        {
            var serializedObject = new byte[0];
            T result;

            using (var decryptor = this.SymmetricAlgorithm.CreateDecryptor(key, iv))
            {
                using (var targetStream = new MemoryStream(obj))
                {
                    using (var cryptoStream = new CryptoStream(targetStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new BinaryReader(cryptoStream, Encoding.ASCII))
                        {
                            byte[] buffer;
                            do
                            {
                                buffer = reader.ReadBytes(SecurityServiceBase.BufferSize);
                                int oldLength = serializedObject.Length;
                                Array.Resize(ref serializedObject, serializedObject.Length + buffer.Length);
                                Array.Copy(buffer, 0, serializedObject, oldLength, buffer.Length);
                            }
                            while (buffer.Length == SecurityServiceBase.BufferSize);
                        }
                    }
                }
            }

            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(serializedObject, 0, serializedObject.Length);
                memoryStream.Position = 0;
                var binaryFormatter = new BinaryFormatter();
                result = binaryFormatter.Deserialize(memoryStream) as T;
            }

            return result;
        }
    }
}
