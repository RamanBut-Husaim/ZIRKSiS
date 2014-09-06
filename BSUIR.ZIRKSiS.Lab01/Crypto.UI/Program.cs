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

            if (args.Length >= 4)
            {
                SymmetricCipher cipher;
                if (Enum.TryParse(args[1], true, out cipher))
                {
                    SymmetricAlgorithm symmetricAlgorithm = cipherProvider.GetSymmetricCipher(cipher);
                    AsymmetricAlgorithm asymmetricAlgorithm = cipherProvider.GetAsymmetricCipher(AsymmetricCipher.RSA);
                    IPasswordProviderBuilder passwordProviderBuilder = new SessionPasswordProviderBuilder(asymmetricAlgorithm);
                    IPasswordProvider passwordProvider = passwordProviderBuilder.Build();
                    if ("encrypt".Equals(args[0], StringComparison.OrdinalIgnoreCase))
                    {
                        using (ICryptoManager cryptoManager = CryptoManager.Create(symmetricAlgorithm, hashAlgorithm, passwordProvider))
                        {
                            if (args.Length == 4)
                            {
                                cryptoManager.Encrypt(args[2], args[3]);
                            }
                            else
                            {
                                cryptoManager.Encrypt(args[2], args[3], args[4]);
                            }
                        }
                    }
                    else
                    {
                        using (ICryptoManager cryptoManager = CryptoManager.Create(symmetricAlgorithm, hashAlgorithm, passwordProvider))
                        {
                            if (args.Length == 4)
                            {
                                cryptoManager.Decrypt(args[2], args[3]);
                            }
                            else
                            {
                                cryptoManager.Decrypt(args[2], args[3], args[4]);
                            }
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
            Console.WriteLine(SymmetricCipher.AES);
            Console.WriteLine(SymmetricCipher.DES);
            Console.WriteLine(SymmetricCipher.TripleDES);
            Console.WriteLine(SymmetricCipher.RC2);
            Console.WriteLine(SymmetricCipher.Rijndael);
        }
    }
}
