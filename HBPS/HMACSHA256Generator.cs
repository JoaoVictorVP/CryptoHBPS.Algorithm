using System.Security.Cryptography;

namespace CryptoHBPS;

/// <summary>
/// A common hash generator using <see cref="HMACSHA256"/>
/// </summary>
public struct HMACSHA256Generator : IHashGenerator
{
    public int Size => 32;
    public void GetHash(Span<byte> key, Span<byte> data, Span<byte> destination)
    {
        HMACSHA256.HashData(key, data, destination);
    }
}