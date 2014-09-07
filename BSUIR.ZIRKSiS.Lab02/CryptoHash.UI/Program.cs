using System;
using CryptoHash.Hashing;
using CryptoHash.Providers;

namespace CryptoHash.UI
{
    public sealed class Program
    {
        //0 - mode h- hash, sh - secureHash, ds - digitalSignature
        //1 - file to hash
        //2 - hash file
        //3 - password
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                if ("h".Equals(args[0], StringComparison.OrdinalIgnoreCase))
                {
                    ProcessHashMode(args);
                }
                else if ("sh".Equals(args[0], StringComparison.OrdinalIgnoreCase))
                {
                    ProcessSecureHashMode(args);
                }

            }
            else
            {
                Console.WriteLine("Not eanough parameters.");
            }
        }

        static void ProcessHashMode(string[] args)
        {
            IHashAlgorithmProviderBuilder hashAlgorithmProviderBuilder = new HashAlgorithmProviderBuilder();

            using (var hashAlgorithm = hashAlgorithmProviderBuilder.Build().GetHashAlgorithm(StandardHashAlgorithm.SHA512))
            {
                using (var passwordProvider = RandomPasswordProvider.Create())
                {
                    IHashingManagerBuilder hashingManagerBuilder = new HashingManagerBuilder();
                    using (var hashingManager = hashingManagerBuilder.Build(hashAlgorithm, passwordProvider))
                    {
                        hashingManager.ComputeHash(args[1], args[2], args[3]);
                    }
                }
            }
        }

        static void ProcessSecureHashMode(string[] args)
        {
            IHashAlgorithmProviderBuilder hashAlgorithmProviderBuilder = new HashAlgorithmProviderBuilder();
            using (var hashAlgorithm = hashAlgorithmProviderBuilder.Build().GetSecureHashAlgorithm(HMACHashAlgorithm.HMACSHA512))
            {
                using (var passwordProvider = RandomPasswordProvider.Create())
                {
                    IHashingManagerBuilder hashingManagerBuilder = new HashingManagerBuilder();
                    using (var hashingManager = hashingManagerBuilder.BuildSecure(hashAlgorithm, passwordProvider))
                    {
                        hashingManager.ComputeHash(args[1], args[2], args[3]);
                    }
                }
            }
        }
    }
}
