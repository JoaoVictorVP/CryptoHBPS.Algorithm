namespace CryptoHBPS;

/// <summary>
/// Interface for any HashGenerator's that an HBPS can use
/// </summary>
public interface IHashGenerator
{
    /// <summary>
    /// The size in bytes of this hashing algorithm
    /// </summary>
    public int Size { get; }
    /// <summary>
    /// Get's a hash for the associated data using key in a HMAC encryption scheme (idealy)
    /// </summary>
    /// <param name="key">The key used to generate hash</param>
    /// <param name="data">The data to be hashed</param>
    /// <param name="destination">The destination of generated hash.<br/>Must be <see cref="Size"/> size</param>
    public void GetHash(Span<byte> key, Span<byte> data, Span<byte> destination);
}
