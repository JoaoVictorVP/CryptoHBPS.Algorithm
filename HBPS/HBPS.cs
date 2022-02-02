using System.Security.Cryptography;

namespace CryptoHBPS;

/// <summary>
/// Hash-Based Property Signature (or System, as you think fit better)
/// </summary>
public class HBPS
{
    /// <summary>
    /// 256-bit security master key size
    /// </summary>
    public const int MasterKeySize = 32;
    /// <summary>
    /// 256-bit security private key size
    /// </summary>
    public const int KeySize = 32;
    /// <summary>
    /// 256-bit security public key size
    /// </summary>
    public const int PublicKeySize = 32;

    /// <summary>
    /// This HBPS random generator instance
    /// </summary>
    public readonly IHBPSRandomGenerator RandomGenerator;
    /// <summary>
    /// This HBPS hash generator instance
    /// </summary>
    public readonly IHashGenerator HashGenerator;

    /// <summary>
    /// Get a public/private key pair from current generator in order to use as property
    /// </summary>
    /// <returns></returns>
    public (Key key, PublicKey pkey) Get()
    {
        Span<byte> keySpan = stackalloc byte[KeySize];
        // Draw a random private key K from the generator
        RandomGenerator.Fill(keySpan);
        Span<byte> pkeySpan = stackalloc byte[PublicKeySize];

        // Derivate public key PK from K feeding as key and data for hash
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
        // Signs the firs part of message with public key (used to verify that it was got from a valid public key later
        HashGenerator.GetHash(pkey.AsSpan(), data, signature[..hashSize]);
        // Signs the second part of message with first part and private key, used to prove authenticity later when private key K is commited
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
        // Make a signature from private and public key over data, if this signature matches the provided one, the provided one is really signed with this private and public key
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
        // Verify if the first part of this provided signature matches the signature made with the given public key, if true, then the signature was really made with this public key
        return signature[..hashSize].SequenceEqual(psign);
    }

    /// <summary>
    /// Create's a new instance of HBPS using the specified <see cref="IHBPSRandomGenerator"/> and <see cref="IHashGenerator"/>
    /// </summary>
    /// <param name="randomGenerator">Random generator, used to generate private keys</param>
    /// <param name="hashGenerator">Hash generator, used to derivate public keys</param>
    public HBPS(IHBPSRandomGenerator randomGenerator, IHashGenerator hashGenerator)
    {
        RandomGenerator = randomGenerator;
        HashGenerator = hashGenerator;
    }
}
