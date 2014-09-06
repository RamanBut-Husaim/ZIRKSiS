using System;
using System.Security.Cryptography;
using Crypto.Ciphering;
using Crypto.Providers;
using Crypto.Providers.Ciphers;

namespace Crypto.UI
{
    public sealed class Program
    {
        //0 - mode
        //1 - algorithm
        //2 - input file
        //3 - output file
        //4 - password
        static void Main(string[] args)
        {
            DisplayAlgorithms();

            IHashAlgorithmProviderStandard hashAlgorithmProvider = new HashAlgorithmProviderStandardBuilder().Build();
            HashAlgorithm hashAlgorithm = hashAlgorithmProvider.GetHashAlgorithm(HashAlgorithms.SHA512);
            ICipherProviderStandard cipherProvider = new CipherProviderStandardBuilder().Build();

            if (args.Length == 5)
            {
                SymmetricCiphers cipher;
                if (Enum.TryParse(args[1], true, out cipher))
                {
                    var symmetricAlgorithm = cipherProvider.GetSymmetricCipher(cipher);
                    if ("encrypt".Equals(args[0], StringComparison.OrdinalIgnoreCase))
                    {
                        using (ICryptoManager cryptoManager = CryptoManager.Create(symmetricAlgorithm, hashAlgorithm))
                        {
                            cryptoManager.Encrypt(args[2], args[3], args[4]);
                        }
                    }
                    else
                    {
                        using (ICryptoManager cryptoManager = CryptoManager.Create(symmetricAlgorithm, hashAlgorithm))
                        {
                            cryptoManager.Decrypt(args[2], args[3], args[4]);
                        }
                    }

                    Console.WriteLine("The operation has been completed successfully.");
                }
            }
            else
            {
                Console.WriteLine("Not enough parameters.");
            }
        }

        private static void DisplayAlgorithms()
        {
            Console.WriteLine("Available algorithms: ");
            Console.WriteLine(SymmetricCiphers.AES);
            Console.WriteLine(SymmetricCiphers.DES);
            Console.WriteLine(SymmetricCiphers.TripleDES);
            Console.WriteLine(SymmetricCiphers.RC2);
            Console.WriteLine(SymmetricCiphers.Rijndael);
        }
    }
}
