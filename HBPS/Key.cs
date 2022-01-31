namespace CryptoHBPS;

public unsafe struct Key
{
    public fixed byte data[HBPS.KeySize];

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

    public override string ToString() => Convert.ToHexString(AsSpan());
    public static Key FromString(string hexString) => new Key(Convert.FromHexString(hexString));
}
