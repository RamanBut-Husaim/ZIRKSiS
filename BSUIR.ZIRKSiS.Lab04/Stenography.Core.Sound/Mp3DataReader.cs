using System;
using System.Linq;
using Stenography.Core.Contract;

namespace Stenography.Core.Sound
{
    public sealed class Mp3DataReader : IMp3DataReader
    {
        private readonly IMp3DataAnalyzer _mp3DataAnalyzer;

        //the number of parts in whuch the byte is divided
        private const int PartNumber = 4;

        private byte[] _fileData;

        private bool _disposed;

        public Mp3DataReader(IMp3DataAnalyzer mp3DataAnalyzer)
        {
            this._mp3DataAnalyzer = mp3DataAnalyzer;
            
        }

        public void Init(byte[] fileData)
        {
            this._fileData = fileData.Clone() as byte[];
            this._mp3DataAnalyzer.Init(fileData);
        }

        public int ReadLength()
        {
            var frame = this._mp3DataAnalyzer.First();

            var lengthInBytes = new byte[sizeof(int)];

            for (int i = 0; i < lengthInBytes.Length; ++i)
            {
                lengthInBytes[i] = this.ReadByte(frame, i);
            }

            var result = lengthInBytes.ToInt();

            return result;
        }

        public byte[] ReadData(int length)
        {
            int bufferPosition = 0;
            var buffer = new byte[length];
            foreach (MPEGVersionFrameDescriptor frameHeader in this._mp3DataAnalyzer.Skip(1))
            {
                int availableBytes = (frameHeader.FrameSize - frameHeader.HeaderSize) / PartNumber;
                int currentPosition = bufferPosition;
                for (int i = 0; bufferPosition < Math.Min(length, currentPosition + availableBytes); ++i, ++bufferPosition)
                {
                    buffer[bufferPosition] = this.ReadByte(frameHeader, i);
                }
            }

            return buffer;
        }

        private byte ReadByte(MPEGVersionFrameDescriptor frameHeader, int index)
        {
            int startPosition = frameHeader.StartIndex + frameHeader.HeaderSize + (index * PartNumber);
            byte result = 0;
            for (int i = 0; i < PartNumber; ++i)
            {
                result |= (byte)((this._fileData[startPosition + i] & 0x03) << ((PartNumber - i - 1) * 2));
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
                    this._fileData = null;
                }
            }
        }
    }
}
