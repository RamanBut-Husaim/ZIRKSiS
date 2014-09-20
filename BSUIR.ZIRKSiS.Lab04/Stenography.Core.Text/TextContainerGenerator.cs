using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Stenography.Core.Contract;

namespace Stenography.Core.Text
{
    public sealed class TextContainerGenerator : IDisposable
    {
        private const int ShortWordLimit = 4;
        private readonly RNGCryptoServiceProvider _rngCryptoServiceProvider;
        private readonly ITextDataProcessor _textDataProcessor;
        private TextDataProcessingResult _textDataProcessingResult;
        private bool _disposed;

        public TextContainerGenerator(ITextDataProcessor textDataProcessor)
        {
            this._textDataProcessor = textDataProcessor;
            this._rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        public async Task Init()
        {
            if (this._textDataProcessingResult == null)
            {
                this._textDataProcessingResult = await this._textDataProcessor.ProcessAsync();
            }
        }

        public async Task<string> GenerateTextContainerAsync(string phrase)
        {
            await this.Init();
            var resultPhrase = new StringBuilder();

            foreach (var @char in phrase.Replace(" ", string.Empty))
            {
                var buffer = new byte[sizeof(int)];
                this._rngCryptoServiceProvider.GetNonZeroBytes(buffer);
                string senseWord = this._textDataProcessingResult.WordLists.ContainsKey(@char)
                                       ? this._textDataProcessingResult.WordLists[@char][Math.Abs(buffer.ToInt()) % this._textDataProcessingResult.WordLists[@char].Count]
                                       : "{unknown}";
                this._rngCryptoServiceProvider.GetNonZeroBytes(buffer);
                resultPhrase.Append(senseWord);
                resultPhrase.Append(' ');
                for (int i = 0; i < buffer.ToInt() % ShortWordLimit; ++i)
                {
                    this._rngCryptoServiceProvider.GetNonZeroBytes(buffer);
                    string senselessWord = this._textDataProcessingResult.ShortWords.Count > 0
                                               ? this._textDataProcessingResult.ShortWords[Math.Abs(buffer.ToInt()) % this._textDataProcessingResult.ShortWords.Count]
                                               : "{un}";
                    resultPhrase.Append(senselessWord);
                    resultPhrase.Append(' ');
                }
            }

            return resultPhrase.ToString();
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
                    this._rngCryptoServiceProvider.Dispose();
                    this._disposed = true;
                }
            }
        }
    }
}
