namespace CryptoHBPS;

public interface IHBPSRandomGenerator
{
    public void Fill(Span<byte> bytes);
}
