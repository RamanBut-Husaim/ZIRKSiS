using System;
using Stenography.Core.Contract;

namespace Stenography.Core.Sound
{
    public sealed class FrameDescriptorBuilder : IFrameDescriptorBuilder
    {
        private const int AudioVersionShift = 19;

        private const int AudioVersionMask = 0x3;

        internal FrameDescriptorBuilder()
        {
        }

        public MPEGVersionFrameDescriptor Build(byte[] headerBytes)
        {
            int header = headerBytes.ToInt();
            var audioVersion = (byte)((header & (AudioVersionMask << AudioVersionShift)) >> AudioVersionShift);
            if (audioVersion == 0)
            {
                return new MPEG25FrameDescriptor(headerBytes);
            }

            if (audioVersion == 1)
            {
                return new MPEG2FrameDescriptor(headerBytes);
            }

            if (audioVersion == 3)
            {
                return new MPEG1FrameDescriptor(headerBytes);
            }

            throw new NotSupportedException("Invalid frame header.");
        }

        public static IFrameDescriptorBuilder Create()
        {
            return new FrameDescriptorBuilder();
        }
    }
}
