using System;

namespace Stenography.Core
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

            for (int i = startIndex; i < Math.Min(startIndex + Math.Min(count, 4), @this.Length); ++i)
            {
                result |= @this[i] << (i * 8);
            }

            return result;
        }
    }
}
