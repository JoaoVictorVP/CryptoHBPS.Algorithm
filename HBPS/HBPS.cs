using System.Security.Cryptography;

namespace CryptoHBPS;

public class HBPS
{
    /// <summary>
    /// 256-bit security
    /// </summary>
    public const int KeySize = 32;
    public const int PublicKeySize = 32;

    public IHBPSRandomGenerator RandomGenerator;
    public IHashGenerator HashGenerator;

    public (Key key, PublicKey pkey) Get()
    {
        Span<byte> keySpan = stackalloc byte[KeySize];
        RandomGenerator.Fill(keySpan);
        Span<byte> pkeySpan = stackalloc byte[PublicKeySize];

        HashGenerator.GetHash(keySpan, keySpan, pkeySpan);
        //HMACSHA256.HashData(keySpan, keySpan, pkeySpan);

        return (new Key(keySpan), new PublicKey(pkeySpan));
    }

    public HBPS(IHBPSRandomGenerator randomGenerator)
    {
        RandomGenerator = randomGenerator;
    }
}
