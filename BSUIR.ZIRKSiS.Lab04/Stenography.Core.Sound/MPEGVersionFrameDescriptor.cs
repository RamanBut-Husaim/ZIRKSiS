using Stenography.Core.Contract;

namespace Stenography.Core.Sound
{
    public abstract class MPEGVersionFrameDescriptor
    {
        private const int LayerRShirt = 17;

        private const int BitrateIndexRShift = 12;

        private const int BitrateIndexMask = 0xF;

        private const int SamplingRateIndexMask = 0x3;

        private const int PaddingIndexMask = 0x1;

        private const int LayerIndexMask = 0x3;

        private const int PaddingRShift = 9;

        private const int SamplingRateRShift = 11;

        private readonly byte[] _frameHeaderBytes;

        private readonly int _frameHeader;

        private readonly byte _layerIndex;

        private readonly byte _bitrateIndex;

        private readonly bool _padded;

        private readonly byte _samplingRateIndex;

        private int _startIndex;

        protected MPEGVersionFrameDescriptor(byte[] frameHeader)
        {
            this._frameHeaderBytes = frameHeader.Clone() as byte[];
            this._frameHeader = frameHeader.ToInt();
            this._layerIndex = (byte)(3 - ((this._frameHeader & (LayerIndexMask << LayerRShirt)) >> LayerRShirt));
            this._bitrateIndex = (byte)((this._frameHeader & (BitrateIndexMask << BitrateIndexRShift)) >> BitrateIndexRShift);
            this._padded = (byte)((this._frameHeader & (PaddingIndexMask << PaddingRShift)) >> PaddingRShift) == 1;
            this._samplingRateIndex = (byte)((this._frameHeader & (SamplingRateIndexMask << SamplingRateRShift)) >> SamplingRateRShift);
        }

        protected int FrameHeader
        {
            get
            {
                return this._frameHeader;
            }
        }

        protected byte LayerIndex
        {
            get
            {
                return this._layerIndex;
            }
        }

        protected byte BitrateIndex
        {
            get
            {
                return this._bitrateIndex;
            }
        }

        protected bool Padded
        {
            get
            {
                return this._padded;
            }
        }

        protected byte SamplingRateIndex
        {
            get
            {
                return this._samplingRateIndex;
            }
        }

        public byte SlotSize
        {
            get
            {
                return this.LayerIndex == 0 ? (byte)4 : (byte)1;
            }
        }

        public int Padding
        {
            get
            {
                return this.Padded ? this.SlotSize : 0;
            }
        }

        public int FrameSize
        {
            get
            {
                return this.LayerIndex == 0
                           ? (12 * this.Bitrate / this.SamplingRate + this.Padding) * 4
                           : (144 * this.Bitrate / this.SamplingRate + this.Padding);
            }
        }

        public int StartIndex
        {
            get
            {
                return _startIndex;
            }

            set
            {
                _startIndex = value;
            }
        }

        public abstract int Bitrate { get; }
       
        public abstract int SamplingRate { get; }
    }
}
