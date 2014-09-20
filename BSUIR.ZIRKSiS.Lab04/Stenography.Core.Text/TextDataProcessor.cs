using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Stenography.Core.Text
{
    public sealed class TextDataProcessor : ITextDataProcessor
    {
        private readonly int _targetPosition;
        private readonly string _targetDirectory;
        private readonly IDictionary<char, HashSet<string>> _words;
        private readonly HashSet<string> _shortWords; 

        public TextDataProcessor(string targetDirectory, int targetPosition = 4)
        {
            this._targetDirectory = targetDirectory;
            this._targetPosition = targetPosition;
            this._words = new Dictionary<char, HashSet<string>>();
            this._shortWords = new HashSet<string>();
        }

        public async Task<TextDataProcessingResult> ProcessAsync()
        {
            IList<Task<TextProcessingResult>> tasks = Directory.GetFiles(this._targetDirectory, "*.txt").Select(
                file => Task.Run(
                    async () =>
                    {
                        using (var splitter = new EnglishWordSplitter(file))
                        {
                            var wordProcessor = new WordGroupProcessor(this._targetPosition, splitter);
                            var result = await wordProcessor.ProcessAsync();
                            return result;
                        }
                    })).ToList();

            foreach (Task<TextProcessingResult> task in tasks)
            {
                TextProcessingResult result = await task;
                this.ProcessCharSets(result);
                this._shortWords.UnionWith(result.ShortWords);
            }

            return
                new TextDataProcessingResult(
                    this._words.ToDictionary<KeyValuePair<char, HashSet<string>>, char, IList<string>>(
                        wordList => wordList.Key,
                        wordList => wordList.Value.ToList()),
                    this._shortWords.ToList());
        }

        private void ProcessCharSets(TextProcessingResult result)
        {
            foreach (var charSet in result.CharSets)
            {
                if (this._words.ContainsKey(charSet.Key))
                {
                    this._words[charSet.Key].UnionWith(charSet.Value);
                }
                else
                {
                    this._words.Add(charSet.Key, charSet.Value);
                }
            }
        }
    }
}
