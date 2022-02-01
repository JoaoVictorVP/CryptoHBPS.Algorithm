using System.Security.Cryptography;

namespace CryptoHBPS;

public struct HMACSHA256Generator : IHashGenerator
{
    public int Size => 32;
    public void GetHash(Span<byte> key, Span<byte> data, Span<byte> destination)
    {
        HMACSHA256.HashData(key, data, destination);
    }
}