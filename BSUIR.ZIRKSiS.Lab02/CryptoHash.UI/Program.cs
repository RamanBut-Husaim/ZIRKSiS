using System;
using CryptoHash.Hashing;
using CryptoHash.Providers;

namespace CryptoHash.UI
{
    public sealed class Program
    {
        //0 - file to hash
        //1 - hash file
        //2 - password
        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                IHashAlgorithmProviderBuilder hashAlgorithmProviderBuilder = new HashAlgorithmProviderBuilder();

                using (var hashAlgorithm = hashAlgorithmProviderBuilder.Build().GetHashAlgorithm(StandardHashAlgorithm.SHA512))
                {
                    using (var passwordProvider = RandomPasswordProvider.Create())
                    {
                        IHashingManagerBuilder hashingManagerBuilder = new HashingManagerBuilder();
                        using (var hashingManager = hashingManagerBuilder.Build(hashAlgorithm, passwordProvider))
                        {
                            hashingManager.ComputeHash(args[0], args[1], args[2]);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Not eanough parameters.");
            }
        }
    }
}
