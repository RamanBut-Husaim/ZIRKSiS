using System.Threading.Tasks;

namespace Stenography.Core.Text
{
    public interface ITextDataProcessor
    {
        Task<TextDataProcessingResult> ProcessAsync();
    }
}
