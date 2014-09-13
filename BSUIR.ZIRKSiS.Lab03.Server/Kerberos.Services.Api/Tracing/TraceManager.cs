using System;
using System.IO;
using System.Text;
using Kerberos.Services.Api.Contracts;
using Kerberos.Services.Api.Contracts.Authentication;
using Kerberos.Services.Api.Contracts.Tracing;

namespace Kerberos.Services.Api.Tracing
{
    public sealed class TraceManager : ITraceManager
    {
        private readonly StreamWriter _streamWriter;
        private bool _disposed;

        public TraceManager(string pathToFile)
        {
            this._streamWriter = new StreamWriter(pathToFile, false, Encoding.UTF8);
        }

        public void TraceAuthRequest(string operationName, IAuthenticationRequest authenticationRequest)
        {
            this.TraceStart(operationName);

            this._streamWriter.WriteLine("---------------------------------------------------------");
            this._streamWriter.WriteLine("Authentication request: ");
            this._streamWriter.WriteLine("UserId: " + authenticationRequest.UserId);
            this._streamWriter.WriteLine("Server Id: " + authenticationRequest.ServerId);
            this._streamWriter.WriteLine("Time stamp: " + authenticationRequest.TimeStamp);
            this._streamWriter.WriteLine("---------------------------------------------------------");

            this.TraceEnd(operationName);
        }

        public void TraceTgsAuthenticationReply(string operationName, ITgsToken tgsToken, ITgtToken tgtToken)
        {
            this.TraceStart(operationName);

           this.TraceTgsInternal(tgsToken);

            this._streamWriter.WriteLine("---------------------------------------------------------");
            this._streamWriter.WriteLine("Tgt token: ");
            this._streamWriter.WriteLine("Client Id: " + tgtToken.ClientId);
            this._streamWriter.WriteLine("IP: " + tgtToken.IpAddress.ToTheString());
            this._streamWriter.WriteLine("Life Stamp: " + new TimeSpan(tgtToken.LifeStamp));
            this._streamWriter.WriteLine("Time Stamp: " + new DateTime(tgtToken.TimeStamp));
            this._streamWriter.WriteLine("Session Key: " + tgtToken.SessionKey.ToTheString());
            this._streamWriter.WriteLine("---------------------------------------------------------");

            this.TraceEnd(operationName);
        }

        public void TraceTgsToken(string operationName, ITgsToken tgsToken)
        {
            this.TraceStart(operationName);

            this.TraceTgsInternal(tgsToken);

            this.TraceEnd(operationName);
        }

        private void TraceTgsInternal(ITgsToken tgsToken)
        {
            this._streamWriter.WriteLine("---------------------------------------------------------");
            this._streamWriter.WriteLine("Tgs token: ");
            this._streamWriter.WriteLine("Id: " + tgsToken.Id);
            this._streamWriter.WriteLine("Lifetime: " + new TimeSpan(tgsToken.LifeTime));
            this._streamWriter.WriteLine("Session Key: " + tgsToken.SessionKey.ToTheString());
            this._streamWriter.WriteLine("---------------------------------------------------------");
        }

        public void TraceByteArray(string operation, byte[] data)
        {
            this._streamWriter.WriteLine("---------------------------------------------------------");
            this._streamWriter.WriteLine(operation + data.ToTheString());
            this._streamWriter.WriteLine("---------------------------------------------------------");
        }

        public void TraceTgsAuthenticationReply(string operationName, byte[] tgsToken, byte[] tgtToken)
        {
            this.TraceStart(operationName);

            this._streamWriter.WriteLine("---------------------------------------------------------");
            this._streamWriter.WriteLine("Tgs encrypted: " + tgsToken.ToTheString());
            this._streamWriter.WriteLine("Tgt encrypted: " + tgtToken.ToTheString());
            this._streamWriter.WriteLine("---------------------------------------------------------");

            this.TraceEnd(operationName);
        }

        private void TraceStart(string operationName)
        {
            this._streamWriter.WriteLine();
            this._streamWriter.WriteLine(operationName + " started");
        }

        private void TraceEnd(string operationName)
        {
            this._streamWriter.WriteLine(operationName + " ended");
            this._streamWriter.WriteLine();
        }

        public void Dispose()
        {
            this.Disposing(true);
        }

        private void Disposing(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._streamWriter.Dispose();
                    this._disposed = true;
                }
            }
        }
    }
}
