using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kerberos.Crypto.Contracts;
using Kerberos.Data.Contracts;
using Kerberos.Models;
using Kerberos.Services.Api.Contracts;
using Kerberos.Services.Api.Contracts.Authorization;
using Kerberos.Services.Api.Contracts.Tracing;

namespace Kerberos.Services.Api.Authorization
{
    public sealed class AuthorizationService : SecurityServiceBase, IAuthorizationService
    {
        private readonly ITraceManager _traceManager;

        private bool _disposed;

        public AuthorizationService(
            IUnitOfWork unitOfWork, 
            ISymmetricAlgorithmProvider symmetricAlgorithmProvider,
            ITraceManager traceManager)
            : base(unitOfWork, symmetricAlgorithmProvider)
        {
            this._traceManager = traceManager;
        }

        public override string ServiceName
        {
            get
            {
                return "AuthorizationService";
            }
        }

        //TODO: move to the separate class (used by client).
        //TODO: Replace session key with TgsToken.
        public byte[] CreateAuthenticator(string userId, byte[] sessionKey)
        {
            var result  = new byte[0];

             IEnumerable<User> users =
               this.UnitOfWork.Repository<User, int>()
                   .Query()
                   .Filter(p => p.Email.Equals(userId, StringComparison.OrdinalIgnoreCase))
                   .Get();

            User user = users.FirstOrDefault();
            if (user != null)
            {
                var authenticator = new Authenticator
                {
                    ClientId = userId,
                    TimeStamp = DateTime.UtcNow.Ticks
                };

                this._traceManager.Trace("Authenticator created:", authenticator);

                result = this.EncryptAuthenticator(user, sessionKey, authenticator);
            }

            return result;
        }

        public IAuthorizationReply Authorize(IAuthorizationRequest authorizationRequest)
        {
            var authorizationReply = new AuthorizationReply();

            ITgtToken tgtToken = this.DecryptTgt(authorizationRequest.TgtBytes);

            this._traceManager.Trace("TGT Decrypted", tgtToken);

            IEnumerable<User> users =
               this.UnitOfWork.Repository<User, int>()
                   .Query()
                   .Filter(p => p.Email.Equals(tgtToken.ClientId, StringComparison.OrdinalIgnoreCase))
                   .Get();

            User user = users.FirstOrDefault();
            if (user != null)
            {
                IAuthenticator authenticator = this.DecryptAuthenticator(user, authorizationRequest.AutheticatorBytes, tgtToken.SessionKey);

                this._traceManager.Trace("Authenticator decrypted", authenticator);

                var sessionKey = new byte[SessionKeyLength];
                IServiceTicket serviceTicket = this.CreateServiceTicket(tgtToken, sessionKey);
                IServiceToken serviceToken = this.CreateServiceToken(sessionKey);
                this._traceManager.Trace("Authorization reply created", serviceTicket, serviceToken);

                authorizationReply.ServiceTicket = this.EncryptServiceTicket(serviceTicket);
                authorizationReply.ServiceToken = this.EncryptServiceToken(serviceToken, tgtToken.SessionKey, user);
                this._traceManager.Trace("Authorization reply encrypted", Tuple.Create(authorizationReply.ServiceTicket, authorizationReply.ServiceToken));
            }

            return authorizationReply;
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

        private byte[] EncryptServiceTicket(IServiceTicket serviceTicket)
        {
            return this.Encrypt(this.ServiceKey, this.ServiceIV, serviceTicket);
        }

        private byte[] EncryptServiceToken(IServiceToken serviceToken, byte[] sessionKey, User user)
        {
            byte[] iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            return this.Encrypt(sessionKey, iv, serviceToken);
        }

        private byte[] EncryptAuthenticator(User user, byte[] sessionKey, IAuthenticator authenticator)
        {
            byte[] iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            return this.Encrypt(sessionKey, iv, authenticator);
        }

        private IServiceTicket CreateServiceTicket(ITgtToken tgtToken, byte[] sessionKey)
        {
            return new ServiceTicket
            {
                ClientId = tgtToken.ClientId,
                IpAddress = tgtToken.IpAddress,
                LifeTime = new TimeSpan(1, 0, 0).Ticks,
                TimeStamp = DateTime.UtcNow.Ticks,
                SessionKey = sessionKey
            };
        }

        private IServiceToken CreateServiceToken(byte[] sessionKey)
        {
            return new ServiceToken
            {
                LifeTime = new TimeSpan(1, 0, 0).Ticks,
                ServiceId = "authentication service",
                SessionKey = sessionKey
            };
        }

        private ITgtToken DecryptTgt(byte[] tgtBytes)
        {
            byte[] key = this.TgsKey;
            byte[] iv = this.TgsIV;

            Trace.WriteLine("TGS DEC: " + key.ToTheString());
            Trace.WriteLine("IV DEC: " + iv.ToTheString());

            return this.Decrypt<ITgtToken>(key, iv, tgtBytes);
        }

        private IAuthenticator DecryptAuthenticator(User user, byte[] authenticatorBytes, byte[] sessionKey)
        {
            var iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            return this.Decrypt<IAuthenticator>(sessionKey, iv, authenticatorBytes);
        }
    }
}
