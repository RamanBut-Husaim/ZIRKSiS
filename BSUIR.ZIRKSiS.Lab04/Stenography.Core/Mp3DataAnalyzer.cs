using System;
using System.Collections.Generic;
using System.IO;

namespace Stenography.Core
{
    public sealed class Mp3DataAnalyzer : ILsbDataAnalyzer
    {
        private const int HeaderPattern = 0xFFE0;

        private bool _disposed;

        private byte[] _fileData;

        private readonly IHeaderParser _headerParser;

        public Mp3DataAnalyzer(string fileName, IHeaderParser headerParser)
        {
            this._fileData = File.ReadAllBytes(fileName);
            this._headerParser = headerParser;
        }

        public Mp3DataAnalyzer(Stream stream, IHeaderParser headerParser)
        {
            this._headerParser = headerParser;
            this.Init(stream);
        }

        public Mp3DataAnalyzer(byte[] fileData, IHeaderParser headerParser)
        {
            this._fileData = fileData.Clone() as byte[];
            this._headerParser = headerParser;
        }

        private void Init(Stream stream)
        {
            this._fileData = new byte[stream.Length];
            stream.Read(this._fileData, 0, this._fileData.Length);
        }

        public AnalyzationInfo Analyze()
        {
            var startIndex = this.GetStartFrameHeaderIndex();
            var result = new AnalyzationInfo(startIndex, 0, 0);
            IList<int> frameHeaders = this.GetFrameHeadersStartIndexes();
            HeaderType headerType = this.AnalyzeFileHeader();

            return result;
        }

        private HeaderType AnalyzeFileHeader()
        {
            if (_headerParser.IsValid(this._fileData))
            {
                return _headerParser.Parse(this._fileData);
            }

            throw new NotSupportedException("The file with empty tag header is not supported.");
        }

        private IList<int> GetFrameHeadersStartIndexes()
        {
            IList<int> result = new List<int>();

            int temp = 0;

            for (int i = 0; i <= this._fileData.Length - 2; ++i)
            {
                int part = this._fileData.ToInt(i, 2);
                int scanResult = this.ScanInt(part);
                temp += scanResult == -1 ? 8 : scanResult;
                if (scanResult != -1)
                {
                    result.Add(temp);
                    temp = temp - scanResult + 8;
                }
            }

            return result;
        }

        private int GetStartFrameHeaderIndex()
        {
            int result = 0;

            for (int i = 0; i <= this._fileData.Length - 2; ++i)
            {
                int part = this._fileData.ToInt(i, 2);
                int scanResult = this.ScanInt(part);
                result += scanResult == -1 ? 8 : scanResult;
                if (scanResult != -1)
                {
                    break;
                }
            }

            return result;
        }

        private int ScanInt(int value)
        {
            int result = -1;

            int temp = value;
            for (int i = 0; i < 1; ++i)
            {
                if (((temp << i) & HeaderPattern) == HeaderPattern)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._disposed = true;
                }
            }
        }
    }
}
