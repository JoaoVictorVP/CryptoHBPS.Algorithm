using System.Security.Cryptography;

namespace CryptoHBPS;

public class HBPSRandomGenerator : IHBPSRandomGenerator
{
    public void Fill(Span<byte> bytes) => RandomNumberGenerator.Fill(bytes);
}
