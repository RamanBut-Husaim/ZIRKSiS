using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stenography.Core.Text
{
    public sealed class EnglishWordSplitter : IWordSplitter, IDisposable
    {
        private static readonly Regex MultipleWhiteSpaceReplacer;
        private static readonly Regex WordSplitter;
        private readonly IFileTextReader _reader;
        private bool _isDisposed;

        static EnglishWordSplitter()
        {
            MultipleWhiteSpaceReplacer = new Regex(@"\s+", RegexOptions.Compiled);
            WordSplitter = new Regex(@"[^\p{L}]*\p{Z}[^\p{L}]*", RegexOptions.Compiled);
        }

        public EnglishWordSplitter(string pathToFile)
        {
            if (string.IsNullOrEmpty(pathToFile))
            {
                throw new ArgumentException("pathToFile");
            }

            this._reader = new FileTextReader(File.OpenRead(pathToFile));
        }

        public EnglishWordSplitter(Stream stream)
        {
            this._reader = new FileTextReader(stream, Encoding.UTF8, true);
        }

        public int Peek()
        {
            return this._reader.Peek();
        }

        public async Task<string[]> SplitStringAsync()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException("EnglishWordSplitter");
            }

            string[] result = null;
            if (this._reader.Peek() != -1)
            {
                string line = await this._reader.ReadLineAsync();
                result = this.SplitLine(line);
            }
            else
            {
                result = new string[0];
            }

            return result;
        }

        public string[] SplitString()
        {
             if (this._isDisposed)
            {
                throw new ObjectDisposedException("EnglishWordSplitter");
            }

            string[] result = null;
            if (this._reader.Peek() != -1)
            {
                string line = this._reader.ReadLine();
                result = this.SplitLine(line);
            }
            else
            {
                result = new string[0];
            }

            return result;
        }

        public async Task<string[]> SplitTextAsync()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException("EnglishWordSplitter");
            }

            string[] result = null;
            if (this._reader.Peek() != -1)
            {
                string line = await this._reader.ReadToEndAsync();
                result = this.SplitLine(line);
            }
            else
            {
                result = new string[0];
            }

            return result;
        }

        private string[] SplitLine(string line)
        {
            var result = new string[0];

            if (!string.IsNullOrEmpty(line))
            {
                string correctedString = MultipleWhiteSpaceReplacer.Replace(line, " ");
                result = WordSplitter.Split(correctedString);
            }

            return result;
        }

        #region Implementation of IDisposable

        public void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this._reader.Close();
                }
            }

            this._isDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion
    }
}
