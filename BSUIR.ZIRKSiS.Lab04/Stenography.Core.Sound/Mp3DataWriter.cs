using System;
using System.IO;
using System.Linq;
using Stenography.Core.Contract;

namespace Stenography.Core.Sound
{
    public sealed class Mp3DataWriter : IMp3DataWriter
    {
        private readonly IMp3DataAnalyzer _mp3DataAnalyzer;

        //the number of parts in whuch the byte is divided
        private const int PartNumber = 4;

        private byte[] _fileData;

        private bool _disposed;

        public Mp3DataWriter(IMp3DataAnalyzer mp3DataAnalyzer)
        {
            this._mp3DataAnalyzer = mp3DataAnalyzer;
            
        }

        public void Init(byte[] fileData)
        {
            this._fileData = fileData.Clone() as byte[];
            this._mp3DataAnalyzer.Init(fileData);
        }

        public void WriteLength(int length)
        {
            var frame = this._mp3DataAnalyzer.First();

            byte[] lengthBytes = length.ToByteArray();

            for (int i = 0; i < lengthBytes.Length; ++i)
            {
                this.WriteByte(frame, lengthBytes[i], i);
            }
        }

        public void WriteData(byte[] buffer)
        {
            int bufferPosition = 0;
            foreach (MPEGVersionFrameDescriptor frameHeader in this._mp3DataAnalyzer.Skip(1))
            {
                int availableBytes = (frameHeader.FrameSize - frameHeader.HeaderSize) / PartNumber;
                int currentPosition = bufferPosition;
                for (int i = 0; bufferPosition < Math.Min(buffer.Length, currentPosition + availableBytes); ++i, ++bufferPosition)
                {
                    this.WriteByte(frameHeader, buffer[bufferPosition], i);
                }
            }
        }

        private void WriteByte(MPEGVersionFrameDescriptor frameHeader, byte @byte, int index)
        {
            int startPosition = frameHeader.StartIndex + frameHeader.HeaderSize + (index * PartNumber);
            for (int i = 0; i < PartNumber; ++i)
            {
                this._fileData[startPosition + i] = (byte)((this._fileData[startPosition + i] & 0xFC) | ((@byte >> ((PartNumber - i - 1) * 2)) & 0x3));
            }
        }

        public void Save(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                stream.Write(this._fileData, 0, this._fileData.Length);
            }
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
                    this._fileData = null;
                    this._disposed = true;
                }
            }
        }
    }
}
