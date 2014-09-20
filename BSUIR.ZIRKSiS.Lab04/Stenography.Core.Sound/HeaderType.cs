namespace Stenography.Core.Sound
{
    public struct HeaderType
    {
        private readonly int _tagSize;
        private readonly string _headerType;

        public HeaderType(int tagSize, string headerType)
        {
            this._tagSize = tagSize;
            this._headerType = headerType;
        }

        public int TagSize
        {
            get { return this._tagSize; }
        }

        public string HeaderTypeName
        {
            get { return this._headerType; }
        }
    }
}
