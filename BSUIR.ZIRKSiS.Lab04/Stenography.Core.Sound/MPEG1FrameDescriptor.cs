using System;

namespace Stenography.Core.Sound
{
    internal sealed class MPEG1FrameDescriptor : MPEGVersionFrameDescriptor
    {
        private static readonly int[][] BitrateLayers;

        private static readonly int[] SamplingRates;

        static MPEG1FrameDescriptor()
        {
            BitrateLayers = new int[][]
            {
                new[] { 0, 32000, 64000, 96000, 128000, 160000, 192000, 224000, 256000, 288000, 320000, 352000, 384000, 416000, 448000 },
                new[] { 0, 32000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 160000, 192000, 224000, 256000, 320000, 384000 },
                new[] { 0, 32000, 40000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 160000, 192000, 224000, 256000, 320000 }
            };
            SamplingRates = new[] { 44100, 48000, 32000 };
        }

        public MPEG1FrameDescriptor(byte[] frameHeader)
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
