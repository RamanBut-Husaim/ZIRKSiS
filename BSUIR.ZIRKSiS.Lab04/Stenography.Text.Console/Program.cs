using System;
using System.IO;
using Stenography.Core.Text;

namespace Stenography.Text.Console
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string currentDirectory = Environment.CurrentDirectory;
                string parent = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
                string path = Path.Combine(parent, "Texts");

                var textDataProcessor = new TextDataProcessor(path);
                var textContainerGenerator = new TextContainerGenerator(textDataProcessor);
                string result = textContainerGenerator.GenerateTextContainerAsync(args[0]).Result;
                System.Console.WriteLine("Result phrase: {0}", result);
                System.Console.ReadLine();
            }
            else
            {
                System.Console.WriteLine("Not enough parameters.");
                System.Console.ReadLine();
            }
        }
    }
}
