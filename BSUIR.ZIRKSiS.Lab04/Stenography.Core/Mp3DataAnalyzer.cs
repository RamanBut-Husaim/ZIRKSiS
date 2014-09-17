using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core
{
    internal sealed class Mp3DataAnalyzer : ILsbDataAnalyzer
    {
        private const int HeaderPattern = 0x7FF;
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

        }

        private int GetStartFrameHeaderIndex()
        {
            int result = 0;

            for (int i = 0; i < this._fileData.Length - 3; ++i)
            {
                int part = this._fileData.ToInt(i, 3);
                int scanResult = this.ScanInt(part);
                result += scanResult == -1 ? 8 : scanResult;
            }

            return result;
        }

        private int ScanInt(int value)
        {
            int result = -1;

            int temp = value;
            for (int i = 0; i < 6; ++i)
            {
                if (((temp >> i) & HeaderPattern) == HeaderPattern)
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
