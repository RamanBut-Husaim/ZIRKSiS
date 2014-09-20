using System;

namespace Stenography.Core.Sound
{
    public sealed class ID3v2HeaderParser : IHeaderParser
    {
        private const int ID3v2HeaderSize = 10;
        private const string HeaderName = "ID3";
        private const int SizeLenght = 4;
        private const int SizeStartIndex = 6;
        private const int SizeMask = 0x7F;
        private string _headerName;
        
        public ID3v2HeaderParser()
        {
        }

        public bool IsValid(byte[] file)
        {
            return this.VerifyHeaderName(file);
        }

        public HeaderType Parse(byte[] fileBytes)
        {
            var buffer = new byte[ID3v2HeaderSize];
            Array.Copy(fileBytes, 0, buffer, 0, ID3v2HeaderSize);
            string header = this.VerifyHeaderName(buffer) ? HeaderName : string.Empty;
            int tagSize = this.GetTagSize(fileBytes);

            return new HeaderType(tagSize, header);
        }

        private bool VerifyHeaderName(byte[] tagHeader)
        {
            var headerNameChars = new[] { (char)tagHeader[0], (char)tagHeader[1], (char)tagHeader[2] };
            return HeaderName.Equals(new string(headerNameChars), StringComparison.OrdinalIgnoreCase);
        }

        private int GetTagSize(byte[] tagHeader)
        {
            var buffer = new byte[SizeLenght];
            Array.Copy(tagHeader, SizeStartIndex, buffer, 0, SizeLenght);
            int result = 0;
            for (int i = 0; i < buffer.Length; ++i)
            {
                result |= buffer[i] << ((SizeLenght - i - 1) * 7);
            }

            return result + ID3v2HeaderSize;
        }
    }
}
