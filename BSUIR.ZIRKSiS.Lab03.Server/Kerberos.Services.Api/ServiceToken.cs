using System;
using Kerberos.Services.Api.Contracts;

namespace Kerberos.Services.Api
{
    [Serializable]
    internal sealed class ServiceToken : IServiceToken
    {
        private byte[] _sessionKey;

        private string _serviceId;

        private long _lifeTime;

        public byte[] SessionKey
        {
            get
            {
                return this._sessionKey;
            }

            internal set
            {
                this._sessionKey = value;
            }
        }

        public string ServiceId
        {
            get
            {
                return this._serviceId;
            }

            internal set
            {
                this._serviceId = value;
            }
        }

        public long LifeTime
        {
            get
            {
                return this._lifeTime;
            }

            internal set
            {
                this._lifeTime = value;
            }
        }
    }
}
