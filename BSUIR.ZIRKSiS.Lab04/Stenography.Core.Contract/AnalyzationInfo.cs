namespace Stenography.Core.Contract
{
    public struct AnalyzationInfo
    {
        private long _availableBits;

        public AnalyzationInfo(long availableBits)
        {
            this._availableBits = availableBits;
        }

        public long AvailableBits
        {
            get
            {
                return this._availableBits;
            }
        }
    }
}
