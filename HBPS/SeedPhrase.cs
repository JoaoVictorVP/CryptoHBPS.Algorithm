using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace CryptoHBPS;

public static class SeedPhrase
{
    /// <summary>
    /// The seed size in word count (a word correspond to two bytes of a key)
    /// </summary>
    public const int SeedSize = 8;

    static string[] words;
    //static HashSet<string> f_words;
    static ushort wordCount;

    public static void BegindAndLoadResources()
    {
        words = File.ReadAllLines("Seed Words.txt");
        //f_words = new HashSet<string>(words);
        wordCount = (ushort)words.Length;
    }

    public static void EndAndClearResources()
    {
        words = null;
        //f_words = null;
        wordCount = 0;
    }

    public unsafe static MasterKey GetMasterKey(string seed)
    {
        Span<byte> seedBytes = stackalloc byte[SeedSize * sizeof(ushort)];
        //Span<byte> master = stackalloc byte[HBPS.MasterKeySize];
        byte* master_ptr = (byte*)NativeMemory.Alloc(HBPS.MasterKeySize);
        Span<byte> master = new Span<byte>(master_ptr, HBPS.MasterKeySize);

        int curCount = 0;
        string word = null;

        void processWord(Span<byte> m)
        {
            ushort index = (ushort)Array.IndexOf(words, word);
            BinaryPrimitives.WriteUInt16LittleEndian(m[(curCount * sizeof(ushort))..((curCount * sizeof(ushort)) + sizeof(ushort))], index);
            word = null;

            curCount++;
        }

        foreach(var c in seed)
        {
            if(c == ' ')
            {
                processWord(seedBytes);
                continue;
            }
            word += c;
        }

        if (word != null)
            processWord(seedBytes);


        SHA256.HashData(seedBytes, master);

        var mkey = new MasterKey(master);

        NativeMemory.Free(master_ptr);

        return mkey;
    }

    /// <summary>
    /// Obtains a random seed phrase with <see cref="SeedSize"/> words
    /// </summary>
    /// <returns></returns>
    public static string GetSeedPhrase()
    {
        var sb = new StringBuilder();

        Span<byte> seed = stackalloc byte[HBPS.KeySize];

        RandomNumberGenerator.Fill(seed);

        for(int i = 0; i < SeedSize; i++)
        {
            ushort index = BinaryPrimitives.ReadUInt16LittleEndian(seed[(i * sizeof(ushort)).. (i * sizeof(ushort) + sizeof(ushort))]);

            index %= wordCount;

            if (index < 0) index = (ushort)-index;
            //if (index >= wordCount) index %= (short)wordCount;

            sb.Append(words[index] + ' ');
        }

        if (sb[sb.Length - 1] == ' ') sb.Remove(sb.Length - 1, 1);

        return sb.ToString();
    }
}