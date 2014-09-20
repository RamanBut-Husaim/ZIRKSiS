using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core.Text
{
    public sealed class FileTextReader : IFileTextReader
    {
        internal const int DefaultBufferSize = 1024;

        private bool _isDisposed;
        private readonly StreamReader _streamReader;

        public Encoding Encoding
        {
            get { return this._streamReader.CurrentEncoding; }
        }

        public bool Closed
        {
            get { return this._isDisposed; }
        }

        public FileTextReader(Stream stream) 
            : this(stream, Encoding.UTF8)
        {
        }

        public FileTextReader(Stream stream, Encoding encoding) 
            : this(stream, encoding, true)
        {
        }

        public FileTextReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
        {
            if (stream == null || encoding == null)
            {
                throw new ArgumentNullException(stream == null ? "stream" : "encoding");
            }
            this._streamReader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks);

        }

        public async Task<string> ReadLineAsync()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException("FileTextReader");
            }

            return await this._streamReader.ReadLineAsync();
        }

        public async Task<string> ReadToEndAsync()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException("FileTextReader");
            }

            return await this._streamReader.ReadToEndAsync();
        }

        public int Peek()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException("FileTextReader");
            }

            return this._streamReader.Peek();
        }

        public string ReadLine()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException("FileTextReader");
            }

            return this._streamReader.ReadLine();
        }

        public string ReadToEnd()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException("FileTextReader");
            }

            return this._streamReader.ReadToEnd();
        }

        #region Implementation of IDisposable

        public void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this._streamReader.Close();
                }

            }

            this._isDisposed = true;
        }

        public void Close()
        {
            this.Dispose(true);
        }

        public void Dispose()
        {
           this.Dispose(true);
        }

        #endregion
    }
}
