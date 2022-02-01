namespace CryptoHBPS;

public unsafe struct MasterKey
{
    public fixed byte data[HBPS.MasterKeySize];

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

    public bool IsEqual(MasterKey other) => AsSpan().SequenceEqual(new Span<byte>(other.data, HBPS.MasterKeySize));

    public override string ToString() => Convert.ToHexString(AsSpan());
    public static Key FromString(string hexString) => new Key(Convert.FromHexString(hexString));
}