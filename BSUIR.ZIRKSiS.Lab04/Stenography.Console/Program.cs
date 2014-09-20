using System;
using System.IO;
using Stenography.Core.Contract;
using Stenography.Core.Sound;

namespace Stenography.Console
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            if ("h".Equals(args[0], StringComparison.OrdinalIgnoreCase))
            {
                if (args.Length > 3)
                {
                    IHeaderParser headerParser = new ID3v2HeaderParser();
                    IFrameDescriptorBuilder frameDescriptorBuilder = FrameDescriptorBuilder.Create();
                    byte[] fileBytes = File.ReadAllBytes(args[1]);
                    byte[] dataToHide = File.ReadAllBytes(args[2]);
                    var dataAnalyzer = new Mp3DataAnalyzer(headerParser, frameDescriptorBuilder);
                    dataAnalyzer.Init(fileBytes);
                    AnalyzationInfo result = dataAnalyzer.Analyze();
                    System.Console.WriteLine("Available bits: {0} ({1} bytes)", result.AvailableBits, result.AvailableBits / 8);
                    System.Console.WriteLine("Length (original): {0}", dataToHide.Length);
                    var mp3DataWriter = new Mp3DataWriter(dataAnalyzer);
                    mp3DataWriter.Init(fileBytes);
                    mp3DataWriter.WriteLength(dataToHide.Length);
                    mp3DataWriter.WriteData(dataToHide);
                    mp3DataWriter.Save(args[3]);
                }
                else
                {
                    System.Console.WriteLine("Not enough arguments.");
                    System.Console.ReadLine();
                }
            }
            else
            {
                if (args.Length > 2)
                {
                    IHeaderParser headerParser = new ID3v2HeaderParser();
                    IFrameDescriptorBuilder frameDescriptorBuilder = FrameDescriptorBuilder.Create();
                    byte[] fileBytes = File.ReadAllBytes(args[1]);
                    var dataAnalyzer = new Mp3DataAnalyzer(headerParser, frameDescriptorBuilder);
                    var mp3DataReader = new Mp3DataReader(dataAnalyzer);
                    mp3DataReader.Init(fileBytes);
                    int length = mp3DataReader.ReadLength();
                    System.Console.WriteLine("Length (original): {0}", length);
                    byte[] hiddenData = mp3DataReader.ReadData(length);
                    using (var stream = new FileStream(args[2], FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        stream.Write(hiddenData, 0, hiddenData.Length);
                    }
                }
                else
                {
                    System.Console.WriteLine("Not enough arguments.");
                    System.Console.ReadLine();
                }
            }
        }
    }
}
