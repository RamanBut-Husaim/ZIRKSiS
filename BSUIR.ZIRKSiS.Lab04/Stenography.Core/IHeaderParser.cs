using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core
{
    public interface IHeaderParser
    {
        bool IsValid(byte[] fileBytes);
        HeaderType Parse(byte[] fileBtes);
    }
}
