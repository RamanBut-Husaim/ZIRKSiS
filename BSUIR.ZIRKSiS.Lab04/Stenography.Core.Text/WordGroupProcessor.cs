using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stenography.Core.Text
{
    public sealed class WordGroupProcessor : IWordGroupProcessor
    {
        private readonly IWordSplitter _wordSplitter;
        private readonly int _targetPosition;
        private bool _disposed;
        private readonly IDictionary<char, HashSet<string>> _wordContainer;
        private readonly IList<string> _shortWords; 

        public WordGroupProcessor(
            int targetPosition,
            IWordSplitter wordSplitter)
        {
            this._wordSplitter = wordSplitter;
            this._targetPosition = targetPosition;
            this._wordContainer = new Dictionary<char, HashSet<string>>();
            this._shortWords = new List<string>();
        }


        public async Task<TextProcessingResult> ProcessAsync()
        {
            while (this._wordSplitter.Peek() != -1)
            {
                string[] words = await this._wordSplitter.SplitStringAsync();
                this.Classify(words);
            }

            return new TextProcessingResult(this._wordContainer, this._shortWords);
        }

        private void Classify(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                if (word.Length >= this._targetPosition)
                {
                    if (this._wordContainer.ContainsKey(word[this._targetPosition - 1]))
                    {
                        this._wordContainer[word[this._targetPosition - 1]].Add(word.ToLowerInvariant());
                    }
                    else
                    {
                        this._wordContainer.Add(word[this._targetPosition - 1], new HashSet<string>());
                    }
                }
                else
                {
                    this._shortWords.Add(word);
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._disposed = true;
                    this._wordSplitter.Dispose();
                }
            }
        }
    }
}
