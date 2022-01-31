namespace CryptoHBPS;

public interface IHashGenerator
{
    public void GetHash(Span<byte> key, Span<byte> data, Span<byte> destination);
}
