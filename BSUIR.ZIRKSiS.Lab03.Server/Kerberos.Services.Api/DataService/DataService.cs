using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Crypto.Contracts;
using Kerberos.Data.Contracts;
using Kerberos.Models;
using Kerberos.Services.Api.Contracts;
using Kerberos.Services.Api.Contracts.DataService;
using Kerberos.Services.Api.Contracts.Tracing;

namespace Kerberos.Services.Api.DataService
{
    public sealed class DataService : SecurityServiceBase, IDataService
    {
        private readonly ITraceManager _traceManager;

        private bool _disposed;

        public DataService(
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
                return "DataService";
            }
        }

        //TODO: move to the separate class (used by client).
        //TODO: Replace session key with Service.
        public byte[] CreateAuthenticator(string userId, byte[] sessionKey)
        {
            var result = new byte[0];

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

        public IDataServiceReply GetAccess(IDataServiceRequest dataServiceRequest)
        {
            var dataServiceReply = new DataServiceReply();
            IServiceTicket serviceTicket = this.DecryptServiceTicket(dataServiceRequest.ServiceTicket);
            this._traceManager.Trace("DS: Service ticket decrypted", serviceTicket);

            IEnumerable<User> users =
              this.UnitOfWork.Repository<User, int>()
                  .Query()
                  .Filter(p => p.Email.Equals(serviceTicket.ClientId, StringComparison.OrdinalIgnoreCase))
                  .Get();

            User user = users.FirstOrDefault();
            if (user != null)
            {
                IAuthenticator authenticator = this.DecryptAuthenticator(user, serviceTicket.SessionKey, dataServiceRequest.Authenticator);
                this._traceManager.Trace("DS: Authenticator decrypted: ", authenticator);
                ITimeStampContainer timeStampContainer = this.CreateTimeStampContainer(authenticator);
                this._traceManager.Trace("DS: Time stamp container created", timeStampContainer);
                dataServiceReply.TimeStamp = this.EncryptTimeStampContainer(timeStampContainer, serviceTicket.SessionKey, user);
                this._traceManager.Trace("DS: Time stamp container encrypted", Tuple.Create(dataServiceReply.TimeStamp));
            }

            return dataServiceReply;
        }

        public ITimeStampContainer DecryptReply(string userId, IDataServiceReply dataServiceReply, byte[] sessionKey)
        {
            ITimeStampContainer result = null;
            IEnumerable<User> users =
              this.UnitOfWork.Repository<User, int>()
                  .Query()
                  .Filter(p => p.Email.Equals(userId, StringComparison.OrdinalIgnoreCase))
                  .Get();

            User user = users.FirstOrDefault();
            if (user != null)
            {
                result = this.DecryptTimeStampContainer(dataServiceReply.TimeStamp, sessionKey, user);
            }

            return result;
        }

        private ITimeStampContainer DecryptTimeStampContainer(byte[] timeStampContainerBytes, byte[] sessionKey, User user)
        {
            byte[] iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            return this.Decrypt<ITimeStampContainer>(sessionKey, iv, timeStampContainerBytes);
        }

        private byte[] EncryptTimeStampContainer(ITimeStampContainer timeStampContainer, byte[] sessionKey, User user)
        {
            byte[] iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            return this.Encrypt(sessionKey, iv, timeStampContainer);
        }

        private ITimeStampContainer CreateTimeStampContainer(IAuthenticator authenticator)
        {
            return new TimeStampContainer
            {
                TimeStamp = (new DateTime(authenticator.TimeStamp, DateTimeKind.Utc)).AddHours(1).Ticks
            };
        }

        private byte[] EncryptAuthenticator(User user, byte[] sessionKey, IAuthenticator authenticator)
        {
            byte[] iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            return this.Encrypt(sessionKey, iv, authenticator);
        }

        private IAuthenticator DecryptAuthenticator(User user, byte[] sessionKey, byte[] authenticatorBytes)
        {
            byte[] iv = this.GenerateIV(user.PasswordHash, user.PasswordSalt);

            return this.Decrypt<IAuthenticator>(sessionKey, iv, authenticatorBytes);
        }

        private IServiceTicket DecryptServiceTicket(byte[] serviceTicketBytes)
        {
            byte[] key = this.ServiceKey;
            byte[] iv = this.ServiceIV;

            return this.Decrypt<IServiceTicket>(key, iv, serviceTicketBytes);
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
    }
}
