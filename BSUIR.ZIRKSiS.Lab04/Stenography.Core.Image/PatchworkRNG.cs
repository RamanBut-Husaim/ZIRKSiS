using System;
using System.Security.Cryptography;
using Stenography.Core.Contract;

namespace Stenography.Core.Image
{
    public sealed class PatchworkRNG : RandomNumberGenerator
    {
        private static byte[] Salt = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        private readonly string _password;
        private Random _random;
        private int _seed;

        public PatchworkRNG(string password)
        {
            this._password = password;
            this.Init(this._password);
        }

        private void Init(string password)
        {
            using (var keyGenerator = new Rfc2898DeriveBytes(password, Salt))
            {
                this._seed = keyGenerator.GetBytes(4).ToInt();
                this._random = new Random(this._seed);
            }
        }

        public override void GetBytes(byte[] data)
        {
            this._random.NextBytes(data);
        }
    }
}
