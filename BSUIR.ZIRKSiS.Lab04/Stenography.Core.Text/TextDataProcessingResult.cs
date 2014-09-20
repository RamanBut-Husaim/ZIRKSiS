using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core.Text
{
    public sealed class TextDataProcessingResult
    {
        public TextDataProcessingResult(
            IDictionary<char, IList<string>> wordList,
            IList<string> shortWords)
        {
            this.WordLists = wordList;
            this.ShortWords = shortWords;
        }

        public IDictionary<char, IList<string>> WordLists { get; private set; }
        public IList<string> ShortWords { get; private set; }
    }
}
