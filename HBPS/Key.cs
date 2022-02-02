namespace CryptoHBPS;

/// <summary>
/// Key with <see cref="HBPS.KeySize"/> size
/// </summary>
public unsafe struct Key
{
    public fixed byte data[HBPS.KeySize];

    /// <summary>
    /// Get byte span from this key
    /// </summary>
    /// <returns></returns>
    public Span<byte> AsSpan()
    {
        fixed(byte* ptr = data)
        return new Span<byte>(ptr, HBPS.KeySize);
    }

    public Key(Span<byte> bytes)
    {
        for(int i = 0; i < HBPS.KeySize; i++)
            data[i] = bytes[i];
    }

    /// <summary>
    /// Is this key equal to other?
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEqual(Key other) => AsSpan().SequenceEqual(new Span<byte>(other.data, HBPS.KeySize));

    /// <summary>
    /// Converts this key to a hex string based one
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Convert.ToHexString(AsSpan());
    /// <summary>
    /// Gets a key from a hex based string
    /// </summary>
    /// <param name="hexString"></param>
    /// <returns></returns>
    public static Key FromString(string hexString) => new Key(Convert.FromHexString(hexString));
}
