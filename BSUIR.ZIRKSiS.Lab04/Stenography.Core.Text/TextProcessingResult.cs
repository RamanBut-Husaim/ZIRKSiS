using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core.Text
{
    public sealed class TextProcessingResult
    {
        public TextProcessingResult(IDictionary<char, HashSet<string>> charSet, IList<string> shortWords)
        {
            this.CharSets = charSet;
            this.ShortWords = shortWords;
        }

        public IDictionary<char, HashSet<string>> CharSets { get; private set; }
        public IList<string> ShortWords { get; private set; }
    }
}
