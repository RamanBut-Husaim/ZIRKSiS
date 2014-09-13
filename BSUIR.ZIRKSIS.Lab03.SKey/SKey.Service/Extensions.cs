using System;

namespace SKey.Service
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this string @this)
        {
            var bytes = new byte[0];

            if (string.IsNullOrEmpty(@this) == false)
            {
                bytes = new byte[@this.Length * sizeof(char)];
                Buffer.BlockCopy(@this.ToCharArray(), 0, bytes, 0, bytes.Length);
            }

            return bytes;
        }

        public static string ToFullString(this byte[] @this)
        {
            var chars = new char[@this.Length / sizeof(char)];
            Buffer.BlockCopy(@this, 0, chars, 0, @this.Length);

            return new string(chars);
        }

        public static bool Compare(this byte[] @this, byte[] other)
        {
            return UnsafeCompare(@this, other);
        }

        private static unsafe bool UnsafeCompare(byte[] a1, byte[] a2)
        {
            if (a1 == null || a2 == null || a1.Length != a2.Length)
            {
                return false;
            }

            fixed (byte* p1 = a1, p2 = a2)
            {
                byte* x1 = p1, x2 = p2;
                int l = a1.Length;
                for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
                {
                    if (*((long*)x1) != *((long*)x2))
                    {
                        return false;
                    }
                }

                if ((l & 4) != 0)
                {
                    if (*((int*)x1) != *((int*)x2))
                    {
                        return false;
                    }

                    x1 += 4;
                    x2 += 4;
                }

                if ((l & 2) != 0)
                {
                    if (*((short*)x1) != *((short*)x2))
                    {
                        return false;
                    }

                    x1 += 2;
                    x2 += 2;
                }

                if ((l & 1) != 0)
                {
                    if (*((byte*)x1) != *((byte*)x2))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
