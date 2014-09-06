using System;

namespace Crypto.Ciphering
{
    public interface IPasswordProvider : IDisposable
    {
        byte[] GetPassword();
    }
}
