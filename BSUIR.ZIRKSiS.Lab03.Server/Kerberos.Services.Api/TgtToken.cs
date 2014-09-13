using System;
using Kerberos.Services.Api.Contracts;

namespace Kerberos.Services.Api
{
    [Serializable]
    internal sealed class TgtToken : ITgtToken
    {
        private string _clientId;

        private byte[] _ipAddress;

        private long _timeStamp;

        private long _lifeStamp;

        private byte[] _sessionKey;

        public string ClientId
        {
            get
            {
                return this._clientId;
            }

            internal set
            {
                this._clientId = value;
            }
        }

        public byte[] IpAddress
        {
            get
            {
                return this._ipAddress;
            }

            internal set
            {
                this._ipAddress = value;
            }
        }

        public long TimeStamp
        {
            get
            {
                return this._timeStamp;
            }

            internal set
            {
                this._timeStamp = value;
            }
        }

        public long LifeStamp
        {
            get
            {
                return this._lifeStamp;
            }

            internal set
            {
                this._lifeStamp = value;
            }
        }

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
    }
}
