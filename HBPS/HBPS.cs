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

    /// <summary>
    /// Signs the input data with private key and associated public key
    /// </summary>
    /// <param name="key">The private key to sign public signature</param>
    /// <param name="pkey">The public key to sign data</param>
    /// <param name="data">The data to be signed</param>
    /// <param name="signature">The signature output, should be twice the size of currently implemented hash algorithm</param>
    public void Sign(Key key, PublicKey pkey, Span<byte> data, Span<byte> signature)
    {
        int hashSize = HashGenerator.Size;
        HashGenerator.GetHash(pkey.AsSpan(), data, signature[..hashSize]);
        HashGenerator.GetHash(key.AsSpan(), signature[..hashSize], signature[hashSize..]);
    }

    /// <summary>
    /// Verify if the given private key signs the signature of the given public key of the specified data
    /// </summary>
    /// <param name="key">The private key</param>
    /// <param name="pkey">The public key</param>
    /// <param name="data">The data that makes signature</param>
    /// <param name="signature">The signature to check against key, pkey and data</param>
    /// <returns></returns>
    public bool IsSignedWithPrivateKey(Key key, PublicKey pkey, Span<byte> data, Span<byte> signature)
    {
        int hashSize = HashGenerator.Size;
        Span<byte> sign = stackalloc byte[hashSize * 2];
        Sign(key, pkey, data, sign);

        return signature.SequenceEqual(sign);
    }

    /// <summary>
    /// Verify if the given hash is signed with the specified public key
    /// </summary>
    /// <param name="pkey">The public key</param>
    /// <param name="data">The data to verify</param>
    /// <param name="signature">The signature to check</param>
    /// <returns></returns>
    public bool IsSignedWithPublicKey(PublicKey pkey, Span<byte> data, Span<byte> signature)
    {
        int hashSize = HashGenerator.Size;

        Span<byte> psign = stackalloc byte[hashSize];
        HashGenerator.GetHash(pkey.AsSpan(), data, psign);
        return signature[hashSize..].SequenceEqual(psign);
    }

    public HBPS(IHBPSRandomGenerator randomGenerator, IHashGenerator hashGenerator)
    {
        RandomGenerator = randomGenerator;
        HashGenerator = hashGenerator;
    }
}
