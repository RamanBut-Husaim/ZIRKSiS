using System.IO;
using System.Security.Cryptography;
using System.Text;
using SKey.Service;

namespace SKey.Console
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            int iterationCount;

            if (args.Length > 0 && int.TryParse(args[0], out iterationCount))
            {
                byte[] hash = args.Length > 1 ? args[1].ToByteArray() : GenerateHash();

                using (var file = new FileStream("result.txt", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (var streaWriter = new StreamWriter(file, Encoding.UTF8))
                    {
                        using (
                            var skeyHashManager = new SKeyHashManager(
                                new SHA512CryptoServiceProvider(),
                                GetTargetHash(iterationCount, hash),
                                iterationCount))
                        {
                            for (int i = 1; i <= iterationCount; ++i)
                            {
                                byte[] temp = GetTargetHash(i, hash);
                                bool compareResult = skeyHashManager.VerifyHash(temp);
                                streaWriter.WriteLine("{0}: {1} - {2}", i, temp.ToFullString(), compareResult);
                            }
                        }
                    }
                }
            }

            System.Console.WriteLine("Completed");
        }

        private static byte[] GenerateHash()
        {
            var buffer = new byte[50];
            using (var rng = new RNGCryptoServiceProvider())
            {
               rng.GetNonZeroBytes(buffer); 
            }

            return buffer;
        }

        private static byte[] GetTargetHash(int iteratioNo, byte[] initialHash)
        {
            var result = initialHash.Clone() as byte[];

            using (var hash = new SHA512CryptoServiceProvider())
            {
                for (int i = 0; i < iteratioNo; ++i)
                {
                    result = hash.ComputeHash(result);
                }
            }

            return result;
        }
    }
}
