namespace CryptoHBPS;

/// <summary>
/// A struct to represent public key
/// </summary>
public unsafe struct PublicKey
{
    /// <summary>
    /// The content of this public key
    /// </summary>
    public fixed byte data[HBPS.PublicKeySize];

    /// <summary>
    /// Get a byte span from this public key
    /// </summary>
    /// <returns></returns>
    public Span<byte> AsSpan()
    {
        fixed (byte* ptr = data)
            return new Span<byte>(ptr, HBPS.PublicKeySize);
    }

    public PublicKey(Span<byte> bytes)
    {
        for (int i = 0; i < HBPS.PublicKeySize; i++)
            data[i] = bytes[i];
    }

    /// <summary>
    /// Is this public key equalt to <paramref name="other"/>?
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEqual(PublicKey other) => AsSpan().SequenceEqual(new Span<byte>(other.data, HBPS.PublicKeySize));

    /// <summary>
    /// To hex string
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Convert.ToHexString(AsSpan());
    /// <summary>
    /// From hex string
    /// </summary>
    /// <param name="hexString"></param>
    /// <returns></returns>
    public static Key FromString(string hexString) => new Key(Convert.FromHexString(hexString));
}
