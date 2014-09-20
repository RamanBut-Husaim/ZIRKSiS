using System;

namespace Stenography.Core.Contract
{
    public static class Extensions
    {
        public static int ToInt(this byte[] @this)
        {
            int result = ToInt(@this, 0, 4);

            return result;
        }

        public static int ToInt(this byte[] @this, int startIndex, int count)
        {
            int result = 0;

            for (int i = 0, counter = startIndex; counter < Math.Min(startIndex + Math.Min(count, 4), @this.Length); i++, counter++)
            {
                result |= @this[counter] << ((count - i - 1) * 8);
            }

            return result;
        }

        public static byte[] ToByteArray(this int @this)
        {
            var result = new byte[sizeof(int)];

            result[0] = (byte)((@this >> 24) & 0xFF);
            result[1] = (byte)((@this >> 16) & 0xFF);
            result[2] = (byte)((@this >> 8) & 0xFF);
            result[3] = (byte)((@this >> 0) & 0xFF);

            return result;
        }
    }
}
