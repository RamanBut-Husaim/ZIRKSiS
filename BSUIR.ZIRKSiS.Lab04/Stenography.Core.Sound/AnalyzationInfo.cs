namespace Stenography.Core.Sound
{
    public struct AnalyzationInfo
    {
        private int _frameNumber;

        private int _availableBits;

        public AnalyzationInfo(int frameNumber, int availableBits)
        {
            this._frameNumber = frameNumber;
            this._availableBits = availableBits;
        }

        public int FrameNumber
        {
            get
            {
                return this._frameNumber;
            }
        }

        public int AvailableBits
        {
            get
            {
                return this._availableBits;
            }
        }
    }
}
