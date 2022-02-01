// Namespace
using CryptoHBPS;

// How to create properly a HBPS instance

// the example here will use a full random one, but you can efectively use key-based if needed

// For random generation of values inside HBPS
var randomGenerator = new HBPSRandomGenerator();

// For hashing inside HBPS (extensible)
var hashGenerator = new HMACSHA256Generator();

var hbps = new HBPS(randomGenerator, hashGenerator);

// To generate a new private key / public key pair
var keyPair = hbps.Get();