namespace CryptoHBPS;

public unsafe struct PublicKey
{
    public fixed byte data[HBPS.PublicKeySize];

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

    public override string ToString() => Convert.ToHexString(AsSpan());
    public static Key FromString(string hexString) => new Key(Convert.FromHexString(hexString));
}