using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core
{
    public struct AnalyzationInfo
    {
        private int _frameNumber;

        private int _frameSize;

        private int _availableBits;

        public AnalyzationInfo(int frameNumber, int frameSize, int availableBits)
        {
            this._frameNumber = frameNumber;
            this._frameSize = frameSize;
            this._availableBits = availableBits;
        }

        public int FrameNumber
        {
            get
            {
                return this._frameNumber;
            }
        }

        public int FrameSize
        {
            get
            {
                return this._frameSize;
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
