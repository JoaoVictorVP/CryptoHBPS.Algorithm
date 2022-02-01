using CryptoHBPS;

SeedPhrase.BegindAndLoadResources();

string seed = SeedPhrase.GetSeedPhrase();


Console.WriteLine("Your seed: " + seed);

var mkey = SeedPhrase.GetMasterKey(seed);

Console.WriteLine("Master Key: " + mkey);

Console.WriteLine();
Console.WriteLine("Type your wallet seed: ");
string wseed = Console.ReadLine();

var wmkey = SeedPhrase.GetMasterKey(wseed);

bool equal = wmkey.IsEqual(mkey);

Console.WriteLine("Wallet Master Key: " + wmkey);
Console.WriteLine("Wallet Master Key Equal to Master Key: " + equal);

SeedPhrase.EndAndClearResources();

KeyBaseHBPSRandomGenerator randomGen = new KeyBaseHBPSRandomGenerator(mkey.AsSpan().ToArray());

HBPS hbps = new HBPS(randomGen, new HMACSHA256Generator());

var pair = hbps.Get();

Console.WriteLine("Signing [1000 BTC] to " + pair.pkey);
Console.WriteLine();

var otherPair = hbps.Get();

var peoplePair = hbps.Get();

Console.WriteLine("From " + pair.pkey + ", Sending [700 BTC] to " + otherPair.pkey);
Console.WriteLine("From " + pair.pkey + ", Sending [300 BTC] to " + peoplePair.pkey);
Console.WriteLine("Proof of " + pair.pkey + " is " + pair.key);

Console.WriteLine();

Console.WriteLine($"Is {pair.pkey} property of {pair.key}? {(hbps.Validate(pair.key, pair.pkey) ? "Yes" : "No")}");



