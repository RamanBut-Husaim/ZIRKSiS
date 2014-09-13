using System;
using Kerberos.Services.Api.Contracts;

namespace Kerberos.Services.Api
{
    [Serializable]
    internal sealed class TgsToken : ITgsToken
    {
        private byte[] _sessionKey;

        private long _lifeTime;

        private TgsToken()
        {
        }

        public byte[] SessionKey
        {
            get
            {
                return this._sessionKey;
            }

            private set
            {
                this._sessionKey = value;
            }
        }

        public string Id { get; private set; }

        public long LifeTime
        {
            get
            {
                return this._lifeTime;
            }

            private set
            {
                this._lifeTime = value;
            }
        }

        public static ITgsToken Create(byte[] sessionKey, string id, long lifeTime)
        {
            return new TgsToken
            {
                SessionKey = sessionKey,
                Id = id,
                LifeTime = lifeTime
            };
        }
    }
}
