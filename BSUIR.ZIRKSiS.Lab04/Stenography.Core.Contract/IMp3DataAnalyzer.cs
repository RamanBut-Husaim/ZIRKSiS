using System.Collections.Generic;

namespace Stenography.Core.Contract
{
    public interface IMp3DataAnalyzer : IEnumerable<MPEGVersionFrameDescriptor>
    {
        AnalyzationInfo Analyze();

        void Init(byte[] fileData);
    }
}
