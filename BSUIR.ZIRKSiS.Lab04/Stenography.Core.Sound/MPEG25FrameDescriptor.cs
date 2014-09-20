using Stenography.Core.Contract;

namespace Stenography.Core.Sound
{
    internal sealed class MPEG25FrameDescriptor : MPEGVersionFrameDescriptor
    {
        private static readonly int[][] BitrateLayers;
        private static readonly int[] SamplingRates;

        static MPEG25FrameDescriptor()
        {
            BitrateLayers = new int[][]
            {
                new[] { 0, 32000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 144000, 160000, 176000, 192000, 224000, 256000 },
                new[] { 0, 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 144000, 160000 },
                new[] { 0, 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 144000, 160000 }
            };
            SamplingRates = new[] { 11025, 12000, 8000 };
        }

        public MPEG25FrameDescriptor(byte[] frameHeader)
            : base(frameHeader)
        {
        }

        public override int Bitrate
        {
            get
            {
                return BitrateLayers[this.LayerIndex][this.BitrateIndex];
            }
        }

        public override int SamplingRate
        {
            get
            {
                return SamplingRates[this.SamplingRateIndex];
            }
        }
    }
}
