using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Stenography.Core.Contract;

namespace Stenography.Core.Sound
{
    public sealed class Mp3DataAnalyzer : IMp3DataAnalyzer
    {
        private const int HeaderPattern = 0xFFE0;
        private const int FrameHeaderSize = 4;
        private readonly IHeaderParser _headerParser;
        private readonly IFrameDescriptorBuilder _frameDescriptorBuilder;
        private bool _disposed;
        private byte[] _fileData;
        private HeaderType _headerType;
        private IList<MPEGVersionFrameDescriptor> _frameDescriptors;

        public Mp3DataAnalyzer(
            IHeaderParser headerParser,
            IFrameDescriptorBuilder frameDescriptorBuilder)
        {
            this._headerParser = headerParser;
            this._frameDescriptorBuilder = frameDescriptorBuilder;
        }

        public void Init(byte[] fileData)
        {
            this._fileData = fileData.Clone() as byte[];
            this._headerType = this.AnalyzeFileHeader();
            this._frameDescriptors = new List<MPEGVersionFrameDescriptor>();
        }

        public void Init(string fileName)
        {
            this.Init(File.ReadAllBytes(fileName));
        }

        public AnalyzationInfo Analyze()
        {
            IList<MPEGVersionFrameDescriptor> frameDescriptors = this.GetFrameDescriptors();
            long availableBits = 0;

            // the first frame is sued to keep file length
            for (int i = 1; i < frameDescriptors.Count; ++i)
            {
                availableBits += (frameDescriptors[i].FrameSize - FrameHeaderSize) * 2;
            }

            return new AnalyzationInfo(availableBits);
        }

        private IList<MPEGVersionFrameDescriptor> GetFrameDescriptors()
        {
            if (this._frameDescriptors.Count == 0)
            {
                bool nextFrameExists = true;
                int startIndex = this._headerType.TagSize;
                while (nextFrameExists)
                {
                    MPEGVersionFrameDescriptor frame;
                    nextFrameExists = this.TryGetNextFrame(startIndex, out frame);
                    if (nextFrameExists)
                    {
                        startIndex = frame.StartIndex + frame.FrameSize;
                        this._frameDescriptors.Add(frame);
                    }
                }
            }

            return this._frameDescriptors;
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

        public IEnumerator<MPEGVersionFrameDescriptor> GetEnumerator()
        {
            return this.GetFrameDescriptors().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
