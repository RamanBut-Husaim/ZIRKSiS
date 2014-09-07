using System.Security.Cryptography;

namespace CryptoHash.Hashing
{
    public sealed class RandomPasswordProvider : IPasswordProvider
    {
        private const int PasswordSize = 50;
        private const int SaltLength = 20;
        private bool _disposed;
        private readonly RNGCryptoServiceProvider _rngCryptoServiceProvider;

        internal RandomPasswordProvider()
        {
            _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public byte[] GetPassword()
        {
            var result = new byte[PasswordSize];
            _rngCryptoServiceProvider.GetBytes(result);
            return result;
        }

        public byte[] GenerateSalt()
        {
            var result = new byte[SaltLength];
            _rngCryptoServiceProvider.GetBytes(result);
            return result;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _rngCryptoServiceProvider.Dispose();
                    _disposed = true;
                }
            }
        }

        public static IPasswordProvider Create()
        {
            return new RandomPasswordProvider();
        }
    }
}