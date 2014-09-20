using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core.Text
{
    public interface ITextDataReader
    {
        bool Closed { get; }
        Encoding Encoding { get; }
        int Peek();
        Task<string> ReadLineAsync();
        Task<string> ReadToEndAsync();
        string ReadLine();
        string ReadToEnd();
        void Close();
        
    }
}
