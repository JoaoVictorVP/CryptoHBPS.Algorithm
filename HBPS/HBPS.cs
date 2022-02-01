using System.Security.Cryptography;

namespace CryptoHBPS;

public class HBPS
{
    /// <summary>
    /// 256-bit security
    /// </summary>
    public const int MasterKeySize = 32;
    public const int KeySize = 32;
    public const int PublicKeySize = 32;

    public IHBPSRandomGenerator RandomGenerator;
    public IHashGenerator HashGenerator;

    /// <summary>
    /// Get a public/private key pair from current generator in order to use as property
    /// </summary>
    /// <returns></returns>
    public (Key key, PublicKey pkey) Get()
    {
        Span<byte> keySpan = stackalloc byte[KeySize];
        RandomGenerator.Fill(keySpan);
        Span<byte> pkeySpan = stackalloc byte[PublicKeySize];

        HashGenerator.GetHash(keySpan, keySpan, pkeySpan);
        //HMACSHA256.HashData(keySpan, keySpan, pkeySpan);

        return (new Key(keySpan), new PublicKey(pkeySpan));
    }

    /// <summary>
    /// Verify if for a given <paramref name="pkey"/> a <paramref name="key"/> is owner of
    /// </summary>
    /// <param name="key">The public key that is supposed to be owner of <paramref name="pkey"/></param>
    /// <param name="pkey">The key to verify if it is property of <paramref name="key"/></param>
    /// <returns></returns>
    public bool Validate(Key key, PublicKey pkey)
    {
        var keySpan = key.AsSpan();
        Span<byte> genKey = stackalloc byte[HBPS.PublicKeySize];
        HashGenerator.GetHash(keySpan, keySpan, genKey);

        return pkey.AsSpan().SequenceEqual(genKey);
    }

    public HBPS(IHBPSRandomGenerator randomGenerator, IHashGenerator hashGenerator)
    {
        RandomGenerator = randomGenerator;
        HashGenerator = hashGenerator;
    }
}
