using System;
using System.IO;
using System.Text;
using Kerberos.Services.Api.Contracts;
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

        public void Trace<T>(string operationName, T obj) where T : class
        {
            this.TraceStart(operationName);

            this.TraceType(obj);

            this.TraceEnd(operationName);
        }

        public void Trace(string message)
        {
            this._streamWriter.WriteLine("---------------------------------------------------------");
            this._streamWriter.WriteLine(message);
            this._streamWriter.WriteLine("---------------------------------------------------------");
        }

        public void Trace<T1, T2>(string operationName, T1 obj1, T2 obj2) where T1 : class where T2 : class
        {
            this.TraceStart(operationName);

            this.TraceType(obj1);

            this.TraceType(obj2);

            this.TraceEnd(operationName);
        }

        private void TraceType<T>(T obj) where T : class
        {
            Type typeDesc = typeof(T);
            this._streamWriter.WriteLine("---------------------------------------------------------");
            this._streamWriter.WriteLine(typeDesc.Name);
            foreach (var property in typeDesc.GetProperties())
            {
                if (property.CanRead)
                {
                    var value = property.GetValue(obj);
                    if (property.PropertyType == typeof(byte[]))
                    {
                        this._streamWriter.WriteLine(property.Name + ": " + ((byte[])value).ToTheString());
                        continue;
                    }

                    if (property.PropertyType == typeof(long) && property.Name.Contains("Life"))
                    {
                        this._streamWriter.WriteLine(property.Name + ": " + new DateTime((long)value, DateTimeKind.Utc));
                        continue;
                    }

                    if (property.PropertyType == typeof(long) && property.Name.Contains("Time"))
                    {
                        this._streamWriter.WriteLine(property.Name + ": " + new TimeSpan((long)value));
                        continue;
                    }

                    this._streamWriter.WriteLine(property.Name + ": " + value.ToString());
                }
            }

            this._streamWriter.WriteLine("---------------------------------------------------------");
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
