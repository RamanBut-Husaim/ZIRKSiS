using System;
using System.Collections.Generic;
using System.IO;
using Stenography.Core.Contract;

namespace Stenography.Core.Sound
{
    public sealed class Mp3DataAnalyzer : ILsbDataAnalyzer
    {
        private const int HeaderPattern = 0xFFE0;

        private const int FrameHeaderSize = 4;

        private bool _disposed;

        private byte[] _fileData;

        private readonly IHeaderParser _headerParser;

        private readonly IFrameDescriptorBuilder _frameDescriptorBuilder;

        public Mp3DataAnalyzer(
            string fileName, 
            IHeaderParser headerParser,
            IFrameDescriptorBuilder frameDescriptorBuilder)
        {
            this._fileData = File.ReadAllBytes(fileName);
            this._headerParser = headerParser;
            this._frameDescriptorBuilder = frameDescriptorBuilder;
        }

        public Mp3DataAnalyzer(
            Stream stream, 
            IHeaderParser headerParser,
            IFrameDescriptorBuilder frameDescriptorBuilder)
        {
            this._headerParser = headerParser;
            this._frameDescriptorBuilder = frameDescriptorBuilder;
            this.Init(stream);
        }

        public Mp3DataAnalyzer(
            byte[] fileData, 
            IHeaderParser headerParser,
            IFrameDescriptorBuilder frameDescriptorBuilder)
        {
            this._fileData = fileData.Clone() as byte[];
            this._headerParser = headerParser;
            this._frameDescriptorBuilder = frameDescriptorBuilder;
        }

        private void Init(Stream stream)
        {
            this._fileData = new byte[stream.Length];
            stream.Read(this._fileData, 0, this._fileData.Length);
        }

        public AnalyzationInfo Analyze()
        {
            HeaderType headerType = this.AnalyzeFileHeader();
            MPEGVersionFrameDescriptor frameDescriptor;

            bool nextFrameExists = this.TryGetNextFrame(headerType.TagSize, out frameDescriptor);
            int startIndex = -1;

            if (nextFrameExists)
            {
                startIndex = frameDescriptor.StartIndex;
            }

            IList<MPEGVersionFrameDescriptor> frameDescriptors = this.GetFrameDescriptors(headerType);

            var result = new AnalyzationInfo(startIndex, 0, 0);

            return result;
        }

        private IList<MPEGVersionFrameDescriptor> GetFrameDescriptors(HeaderType headerType)
        {
            bool nextFrameExists = true;
            int startIndex = headerType.TagSize;
            IList<MPEGVersionFrameDescriptor> result = new List<MPEGVersionFrameDescriptor>();
            while (nextFrameExists)
            {
                MPEGVersionFrameDescriptor frame;
                nextFrameExists = this.TryGetNextFrame(startIndex, out frame);
                if (nextFrameExists)
                {
                    startIndex = frame.StartIndex + frame.FrameSize;
                    result.Add(frame);
                }
            }

            return result;
        } 

        private HeaderType AnalyzeFileHeader()
        {
            if (this._headerParser.IsValid(this._fileData))
            {
                return this._headerParser.Parse(this._fileData);
            }

            throw new NotSupportedException("The file with empty tag header is not supported.");
        }

        private bool TryGetNextFrame(int startIndex, out MPEGVersionFrameDescriptor frameDescriptor)
        {
            frameDescriptor = null;
            var frameStartIndex = this.GetNextFrameStartPosition(startIndex);
            bool result = frameStartIndex != -1;
            if (result)
            {
                var header = new byte[FrameHeaderSize];
                Array.Copy(this._fileData, frameStartIndex, header, 0, FrameHeaderSize);
                frameDescriptor = this._frameDescriptorBuilder.Build(header);
                frameDescriptor.StartIndex = frameStartIndex;
                return true;
            }

            return false;
        }

        private int GetNextFrameStartPosition(int startIndex)
        {
            int result = startIndex;
            bool found = false;
            for (int i = startIndex; i <= this._fileData.Length - 2; ++i)
            {
                int part = this._fileData.ToInt(i, 2);
                int scanResult = this.ScanInt(part);
                result += scanResult == -1 ? 1 : 0;
                if (scanResult != -1)
                {
                    found = true;
                    break;
                }
            }

            return found ? result : -1;
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
