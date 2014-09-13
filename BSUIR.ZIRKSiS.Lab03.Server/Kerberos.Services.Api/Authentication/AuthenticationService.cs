using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Kerberos.Crypto.Contracts;
using Kerberos.Data.Contracts;
using Kerberos.Models;
using Kerberos.Services.Api.Contracts;
using Kerberos.Services.Api.Contracts.Authentication;
using Kerberos.Services.Api.Contracts.Tracing;

namespace Kerberos.Services.Api.Authentication
{
    public sealed class AuthenticationService : SecurityServiceBase, IAuthenticationService
    {
        private readonly bool _priorAuthenticationRequired;

        private bool _disposed;

        private readonly ITraceManager _traceManager;

        public AuthenticationService(
            IUnitOfWork unitOfWork, 
            ISymmetricAlgorithmProvider symmetricAlgorithmProvider,
            ITraceManager traceManager)
            : base(unitOfWork, symmetricAlgorithmProvider)
        {
            this._priorAuthenticationRequired = false;
            this._traceManager = traceManager;
        }

        public override string ServiceName
        {
            get
            {
                return "AuthenticationService";
            }
        }

        public IAuthenticationReply Authenticate(IAuthenticationRequest authenticationRequest)
        {
            var authenticationReply = new AuthenticationReply();

            this._traceManager.TraceAuthRequest("Authentication Request Received", authenticationRequest);

            IEnumerable<User> users =
                this.UnitOfWork.Repository<User, int>()
                    .Query()
                    .Filter(p => p.Email.Equals(authenticationRequest.UserId, StringComparison.OrdinalIgnoreCase))
                    .Get();

            User user = users.FirstOrDefault();
            if (user != null)
            {
                var sessionKey = new byte[SessionKeyLength];
                this.GetRandomBytes(sessionKey);

                ITgsToken tgsToken = this.CreateTgsToken(sessionKey);
                ITgtToken tgtToken = this.CreateTgtToken(user, sessionKey);
                this._traceManager.TraceTgsAuthenticationReply("TGS Generate tokens", tgsToken, tgtToken);
                authenticationReply.TgsBytes = this.EncryptTgsToken(user, tgsToken);
                authenticationReply.TgtBytes = this.EncryptTgtToken(tgtToken);
                this._traceManager.TraceTgsAuthenticationReply("TGS Encrypt tokens", authenticationReply.TgsBytes, authenticationReply.TgtBytes);
            }
            else
            {
                authenticationReply.Message = "User not found.";
            }

            return authenticationReply;
        }

        public ITgsToken DecryptReply(string userId, IAuthenticationReply authenticationReply)
        {
            ITgsToken tgsToken = null;

            IEnumerable<User> users =
               this.UnitOfWork.Repository<User, int>()
                   .Query()
                   .Filter(p => p.Email.Equals(userId, StringComparison.OrdinalIgnoreCase))
                   .Get();

            User user = users.FirstOrDefault();
            if (user != null)
            {
                tgsToken = this.DecryptTgsToken(user, authenticationReply.TgsBytes);
            }

            return tgsToken;
        }

        protected override void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._disposed = true;
                }
            }

            base.Dispose(disposing);
        }

        private ITgsToken CreateTgsToken(byte[] sessionKey)
        {
            string id = "tgs";
            long lifeTime = new TimeSpan(1, 0, 0).Ticks;
            this.GetRandomBytes(sessionKey);

            return TgsToken.Create(sessionKey, id, lifeTime);
        }

        private ITgtToken CreateTgtToken(User user, byte[] sessionKey)
        {
            var tgtToken = new TgtToken
            {
                ClientId = user.Email,
                IpAddress = new byte[] { 0, 1, 2 },
                LifeStamp = new TimeSpan(1, 0, 0).Ticks,
                SessionKey = sessionKey,
                TimeStamp = DateTime.UtcNow.Ticks
            };

            return tgtToken;
        }

        private ITgsToken DecryptTgsToken(User user, byte[] tgsToken)
        {
            byte[] key = this.GenerateKey(user.PasswordHash, user.PasswordSalt);
            byte[] iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            Trace.WriteLine("Dec Key: " + key.ToTheString());
            Trace.WriteLine("Dec IV: " + iv.ToTheString());

            var serializedObject = new byte[0];
            ITgsToken result;

            using (var decryptor = this.SymmetricAlgorithm.CreateDecryptor(key, iv))
            {
                using (var targetStream = new MemoryStream(tgsToken))
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
                result = binaryFormatter.Deserialize(memoryStream) as ITgsToken;
            }

            return result;
        }

        private byte[] EncryptTgsToken(User user, ITgsToken tgsToken)
        {
            byte[] key = this.GenerateKey(user.PasswordHash, user.PasswordSalt);
            byte[] iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            Trace.WriteLine("Enc Key: " + key.ToTheString());
            Trace.WriteLine("Enc IV: " + iv.ToTheString());

            byte[] serializedObject;
            byte[] result;

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, tgsToken);
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
                        }

                        result = targetStream.ToArray();
                    }
                }
            }

            return result;
        }

        private byte[] EncryptTgtToken(ITgtToken tgtToken)
        {
            byte[] serializedObject;
            byte[] result;

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, tgtToken);
                serializedObject = memoryStream.ToArray();
            }

            using (var encryptor = this.SymmetricAlgorithm.CreateEncryptor(this.TgsKey, this.TgsIV))
            {
                using (var targetStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(targetStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new BinaryWriter(cryptoStream))
                        {
                            writer.Write(serializedObject);
                        }

                        result = targetStream.ToArray();
                    }
                }
            }

            return result;
        }
    }
}
