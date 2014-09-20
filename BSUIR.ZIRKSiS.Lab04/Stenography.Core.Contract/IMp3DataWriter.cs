using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core.Contract
{
    public interface IMp3DataWriter : ILsbDataWriter
    {
        void WriteLength(int length);

        void WriteData(byte[] buffer);

        void Save(string fileName);
    }
}
