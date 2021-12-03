using Common;

var lines = Resources.GetResourceFileLines("input.txt");

int numOfBits = lines.First().Length;

static bool OneIsMostFrequent(IEnumerable<string> numbers, int position)
{
    var numWithOne = numbers.Count(n => n[position] == '1');
    var half = (float)numbers.Count() / 2;

    if (numWithOne == half)
    {
        throw new InvalidOperationException("Equal frequencies of 0 and 1");
    }

    return numWithOne > half;
}

// Part 1
int gammaRate = 0;
int epsilonRate = 0;

for (int i = 0; i < numOfBits; i++)
{
    if (OneIsMostFrequent(lines, numOfBits - i - 1))
    {
        gammaRate |= 1 << i;
    }
    else
    {
        epsilonRate |= 1 << i;
    }
}

Console.WriteLine($"Part 1: {gammaRate * epsilonRate}");

// Part 2
static string[] FilterByBit(IEnumerable<string> numbers, int position, bool keepWithOne)
{
    return numbers.Where(x => x[position] == (keepWithOne ? '1' : '0')).ToArray();
}

int BinToDec(string binary)
{
    int dec = 0;
    for (int i = 0; i < numOfBits; i++)
    {
        dec |= (binary[numOfBits - i - 1] - '0') << i;
    }

    return dec;
}

string FindLifeSupportRating(IEnumerable<string> numbers, bool keepMostCommon, bool keepWithOneWhenEqual)
{
    for (int position = 0; position < numOfBits; position++)
    {
        try
        {
            bool more1s = OneIsMostFrequent(numbers, position);

            // More 1s
            //     + most common => keep 1s
            //     + least common => keep 0s
            // More 0s
            //     + most common => keep 0s
            //     + least common => keep 1s
            numbers = FilterByBit(numbers, position, keepWithOne: more1s == keepMostCommon);
        }
        catch (InvalidOperationException)
        {
            numbers = FilterByBit(numbers, position, keepWithOne: keepWithOneWhenEqual);
        }

        if (numbers.Count() == 1)
        {
            return numbers.Single();
        }
    }

    return numbers.Single();
}

var oxygenRate = BinToDec(FindLifeSupportRating(lines, true, true));
var co2Rate = BinToDec(FindLifeSupportRating(lines, false, false));

Console.WriteLine($"Part 1: {oxygenRate * co2Rate}");
