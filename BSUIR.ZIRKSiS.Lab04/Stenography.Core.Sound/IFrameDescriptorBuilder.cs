using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core.Sound
{
    public interface IFrameDescriptorBuilder
    {
        MPEGVersionFrameDescriptor Build(byte[] header);
    }
}
