using System.Diagnostics.CodeAnalysis;

namespace CryptoHBPS;

public class KeyBaseHBPSRandomGenerator : IHBPSRandomGenerator
{
    public byte[] Key;
    public int Rounds;
    Random[] random;

    int maskSize;
    
    int size;

    public void Fill(Span<byte> bytes)
    {
        Span<byte> mask = stackalloc byte[maskSize];
        for (int i = 0; i < maskSize; i++)
            mask[i] = (byte)random[i % size].Next(byte.MinValue, byte.MaxValue);

        var coherence = new Random(mask[0] * mask[1] * mask[2] * mask[3]);

        int count = bytes.Length;
        for (int i = 0; i < count; i++)
            bytes[i] = (byte)(mask[i % maskSize] * coherence.Next());
    }

    void init()
    {
        random = new Random[size];
        for (int i = 0; i < size; i++)
            random[i] = new Random(Key[i] * i);

        maskSize = size * 3;

        for (int round = 0; round < Rounds; round++)
            for (int i = 0; i < maskSize; i++)
                random[i % size].Next();
    }

    public KeyBaseHBPSRandomGenerator([NotNull] byte[] key, int rounds = 0)
    {
        Key = key;
        Rounds = rounds;
        size = key.Length;

        init();
    }
}