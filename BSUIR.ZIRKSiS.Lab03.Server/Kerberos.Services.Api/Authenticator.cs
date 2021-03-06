﻿using System;
using Kerberos.Services.Api.Contracts;

namespace Kerberos.Services.Api
{
    [Serializable]
    public sealed class Authenticator : IAuthenticator
    {
        private string _clientId;

        private long _timeStamp;

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
    }
}
