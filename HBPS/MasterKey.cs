namespace CryptoHBPS;

/// <summary>
/// Master Key
/// </summary>
public unsafe struct MasterKey
{
    /// <summary>
    /// The master key data of <see cref="HBPS.MasterKeySize"/> size
    /// </summary>
    public fixed byte data[HBPS.MasterKeySize];

    /// <summary>
    /// Get's this Master Key as a span of bytes
    /// </summary>
    /// <returns></returns>
    public Span<byte> AsSpan()
    {
        fixed (byte* ptr = data)
            return new Span<byte>(ptr, HBPS.MasterKeySize);
    }

    public MasterKey(Span<byte> bytes)
    {
        for (int i = 0; i < HBPS.MasterKeySize; i++)
            data[i] = bytes[i];
    }

    /// <summary>
    /// Is this Master Key equal to <paramref name="other"/>?
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEqual(MasterKey other) => AsSpan().SequenceEqual(new Span<byte>(other.data, HBPS.MasterKeySize));

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