using System;
using Kerberos.Services.Api.Contracts;

namespace Kerberos.Services.Api
{
    [Serializable]
    internal sealed class TimeStampContainer : ITimeStampContainer
    {
        private long _timeStamp;

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
