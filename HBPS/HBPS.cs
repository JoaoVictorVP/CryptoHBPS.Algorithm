namespace CryptoHBPS;

public class HBPS
{
    /// <summary>
    /// 256-bit security
    /// </summary>
    public const long KeySize = 32;
    public const long PublicKeySize = 32;

    public IHBPSRandomGenerator RandomGenerator;

    public HBPS(IHBPSRandomGenerator randomGenerator)
    {
        RandomGenerator = randomGenerator;
    }
}
