const int seed = 3000;

Console.WriteLine("Default 0 Rounds");
AlgoA(new Random(seed));
Console.WriteLine();

var random = new Random(seed);
Console.WriteLine("10 rounds already");
AlgoA(random);
AlgoA(random);
Console.WriteLine();

random = new Random(seed);
Console.WriteLine("Try continuing from 6 round and forth");
RestoreState(random, 5);
AlgoA(random);

void AlgoA(Random random)
{
    for (int i = 0; i < 5; i++)
        Console.WriteLine("> " + random.Next(30, 90));
}

void RestoreState(Random random, int rounds)
{
    for (int round = 0; round < rounds; round++)
        random.Next();
}

void AContinues(Random random)
{

}

