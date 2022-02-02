using System.Security.Cryptography;

namespace CryptoHBPS;

/// <summary>
/// A comomn random number generator using <see cref="RandomNumberGenerator"/>
/// </summary>
public class HBPSRandomGenerator : IHBPSRandomGenerator
{
    public void Fill(Span<byte> bytes) => RandomNumberGenerator.Fill(bytes);
}
