using System;
using System.Security.Cryptography;
using Stenography.Core.Contract;

namespace Stenography.Core.Image
{
    public sealed class PatchworkTransformService
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly byte _delta;
        private readonly int _iterationNumber;

        public PatchworkTransformService(
            RandomNumberGenerator randomNumberGenerator,
            byte delta = 4,
            int iterationNumber = 20000)
        {
            this._randomNumberGenerator = randomNumberGenerator;
            this._delta = delta;
            this._iterationNumber = iterationNumber;
        }

        public void ApplyTransform(byte[] data)
        {
            int modulo = data.Length / 3;

            for (int i = 0; i < this._iterationNumber; ++i)
            {
                var buffer = new byte[sizeof(int)];
                this._randomNumberGenerator.GetBytes(buffer);
                int firstIndex = Math.Abs(buffer.ToInt()) % modulo;
                this._randomNumberGenerator.GetBytes(buffer);
                int secondIndex = Math.Abs(buffer.ToInt()) % modulo;
                this.IncreaseIntensity(data, firstIndex);
                this.DecreaseIntensity(data, secondIndex);
            }
        }

        public int Analyze(byte[] data)
        {
            int modulo = data.Length / 3;
            int result = 0;
            for (int i = 0; i < this._iterationNumber; ++i)
            {
                var buffer = new byte[sizeof(int)];
                this._randomNumberGenerator.GetBytes(buffer);
                int firstIndex = Math.Abs(buffer.ToInt()) % modulo;
                this._randomNumberGenerator.GetBytes(buffer);
                int secondIndex = Math.Abs(buffer.ToInt()) % modulo;
                result += data[firstIndex * 3] - data[secondIndex * 3];
            }

            return result;
        }

        private void IncreaseIntensity(byte[] data, int index)
        {
            //data[index * 3] = ((int)data[index * 3] + (int)this._delta) > byte.MaxValue ? byte.MaxValue : (byte)(data[index * 3] + this._delta);
            //data[index * 3 + 1] = ((int)data[index * 3 + 1] + (int)this._delta) > byte.MaxValue ? byte.MaxValue : (byte)(data[index * 3 + 1] + this._delta);
            //data[index * 3 + 2] = ((int)data[index * 3 + 2] + (int)this._delta) > byte.MaxValue ? byte.MaxValue : (byte)(data[index * 3 + 2] + this._delta);
            data[index * 3] += this._delta;
            data[index * 3 + 1] += this._delta;
            data[index * 3 + 2] += this._delta;
        }

        private void DecreaseIntensity(byte[] data, int index)
        {
            //data[index * 3] = ((int)data[index * 3] - (int)this._delta) < 0 ? byte.MinValue : (byte)(data[index * 3] - this._delta);
            //data[index * 3 + 1] = ((int)data[index * 3 + 1] - (int)this._delta) < 0 ? byte.MinValue : (byte)(data[index * 3 + 1] - this._delta);
            //data[index * 3 + 2] = ((int)data[index * 3 + 2] - (int)this._delta) < 0 ? byte.MinValue : (byte)(data[index * 3 + 2] - this._delta);
            data[index * 3] -= this._delta;
            data[index * 3 + 1] -= this._delta;
            data[index * 3 + 2] -= this._delta;
        }
    }
}
