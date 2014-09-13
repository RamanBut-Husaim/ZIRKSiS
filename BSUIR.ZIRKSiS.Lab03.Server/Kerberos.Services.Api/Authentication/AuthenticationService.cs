using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

            this._traceManager.Trace("Authentication Request Received", authenticationRequest);

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
                this._traceManager.Trace("TGS Generate tokens", tgsToken, tgtToken);
                authenticationReply.TgsBytes = this.EncryptTgsToken(user, tgsToken);
                authenticationReply.TgtBytes = this.EncryptTgtToken(tgtToken);
                this._traceManager.Trace("TGS Encrypt tokens", Tuple.Create(authenticationReply.TgsBytes, authenticationReply.TgtBytes));
            }
            else
            {
                authenticationReply.Message = "User not found.";
            }

            return authenticationReply;
        }

        //TODO: move to separate class (used by client).
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
                LifeTime = new TimeSpan(1, 0, 0).Ticks,
                SessionKey = sessionKey,
                TimeStamp = DateTime.UtcNow.Ticks
            };

            return tgtToken;
        }

        private ITgsToken DecryptTgsToken(User user, byte[] tgsToken)
        {
            byte[] key = this.GenerateKey(user.PasswordHash, user.PasswordSalt);
            byte[] iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            return this.Decrypt<ITgsToken>(key, iv, tgsToken);
        }

        private byte[] EncryptTgsToken(User user, ITgsToken tgsToken)
        {
            byte[] key = this.GenerateKey(user.PasswordHash, user.PasswordSalt);
            byte[] iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            return this.Encrypt(key, iv, tgsToken);
        }

        private byte[] EncryptTgtToken(ITgtToken tgtToken)
        {
            byte[] key = this.TgsKey;
            byte[] iv = this.TgsIV;

            Trace.WriteLine("TGS ENC: " + key.ToTheString());
            Trace.WriteLine("IV ENC: " + iv.ToTheString());

            return this.Encrypt(key, iv, tgtToken);
        }
    }
}
