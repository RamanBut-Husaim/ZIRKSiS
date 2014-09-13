using System;
using System.Text;

namespace Kerberos.Services.Api.Contracts
{
    public static class CryptoExtensions
    {
        public static byte[] ToByteArray(this string value)
        {
            byte[] result = new byte[0];

            if (!string.IsNullOrEmpty(value))
            {
                result = new byte[value.Length * sizeof(char)];
                Buffer.BlockCopy(value.ToCharArray(), 0, result, 0, result.Length);
            }

            return result;
        }

        public static string ToTheString(this byte[] @this)
        {
            var stringBuilder = new StringBuilder(@this.Length);

            for (int i = 0; i < @this.Length; ++i)
            {
                stringBuilder.Append(@this[i]);
            }

            return stringBuilder.ToString();
        }
    }
}
