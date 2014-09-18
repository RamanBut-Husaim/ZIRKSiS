using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core
{
    public sealed class Mp3DataAnalyzer : ILsbDataAnalyzer
    {
        private const int HeaderPattern = 0xFFE0;

        private bool _disposed;

        private byte[] _fileData;

        public Mp3DataAnalyzer(string fileName)
        {
            this._fileData = File.ReadAllBytes(fileName);
        }

        public Mp3DataAnalyzer(Stream stream)
        {
            this.Init(stream);
        }

        public Mp3DataAnalyzer(byte[] fileData)
        {
            this._fileData = fileData.Clone() as byte[];
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
            var b = _fileData[1629];
            var c = _fileData[1630];
            var frameHeaders = this.GetFrameHeadersStartIndexes();

            return result;
        }

        private IList<int> GetFrameHeadersStartIndexes()
        {
            IList<int> result = new List<int>();

            int temp = 0;

            for (int i = 0; i <= this._fileData.Length - 2; ++i)
            {
                if (i == 1628)
                {
                    int c = i;
                }
                var a = this._fileData[i];
                var b = this._fileData[i + 1];
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
