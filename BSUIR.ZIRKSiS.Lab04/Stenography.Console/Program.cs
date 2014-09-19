using System.IO;
using Stenography.Core;

namespace Stenography.Console
{
   public sealed class Program
    {
        public static void Main(string[] args)
        {
            //var data = new byte[] { 0xF6, 0x7f, 0x73};
            //var dataAnalyzer = new Mp3DataAnalyzer(data);
            //var result2 = dataAnalyzer.Analyze();

            //data = new byte[] { 0x76, 0x6E, 0xFE, 0x6F, 0x4E };
            //dataAnalyzer = new Mp3DataAnalyzer(data);
            //var result3 = dataAnalyzer.Analyze();
            IHeaderParser headerParser = new ID3v2HeaderParser();
            var dataAnalyzer = new Mp3DataAnalyzer(File.ReadAllBytes("track.mp3"), headerParser);
            var result4 = dataAnalyzer.Analyze();
        }
    }
}
