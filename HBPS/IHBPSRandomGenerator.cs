namespace CryptoHBPS;

/// <summary>
/// Interface for any random number generator HBPS can use
/// </summary>
public interface IHBPSRandomGenerator
{
    /// <summary>
    /// Fills the specified byte span with random bytes
    /// </summary>
    /// <param name="bytes"></param>
    public void Fill(Span<byte> bytes);
}
