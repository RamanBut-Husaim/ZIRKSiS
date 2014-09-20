using System;
using System.Threading.Tasks;

namespace Stenography.Core.Text
{
    public interface IWordSplitter : IDisposable
    {
        Task<string[]> SplitStringAsync();
        string[] SplitString();
        Task<string[]> SplitTextAsync();
        int Peek();
    }
}
