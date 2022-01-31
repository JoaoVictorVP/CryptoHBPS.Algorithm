namespace CryptoHBPS;

public class HBPS
{
    /// <summary>
    /// 256-bit security
    /// </summary>
    public const int KeySize = 32;
    public const int PublicKeySize = 32;

    public IHBPSRandomGenerator RandomGenerator;

    public (Key, PublicKey) Get()
    {

    }

    public HBPS(IHBPSRandomGenerator randomGenerator)
    {
        RandomGenerator = randomGenerator;
    }
}
