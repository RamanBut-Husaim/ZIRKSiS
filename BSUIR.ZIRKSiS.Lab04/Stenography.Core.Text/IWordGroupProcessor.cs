using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core.Text
{
    public interface IWordGroupProcessor : IDisposable
    {
        Task<TextProcessingResult> ProcessAsync();
    }
}
