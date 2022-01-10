static long MysteriousOperation(int input, long z, int first, int second)
{
    long x = z % 26;

    if (first < 0)
    {
        z /= 26;
    }

    if (x != input - first)
    {
        z *= 26;
        z += input + second;
    }

    return z;
}

var monadParameters = new[]
{
    (15, 13),
    (10, 16),
    (12, 2),
    (10, 8),
    (14, 11),
    (-11, 6),
    (10, 12),
    (-16, 2),
    (-9, 2),
    (11, 15),
    (-8, 1),
    (-8, 10),
    (-10, 14),
    (-9, 10),
};

static bool Monad(string input, (int, int)[] parameters)
{
    if (input.Contains('0'))
    {
        return false;
    }

    var position = 0;
    int GetNext()
    {
        return input[position++] - '0';
    }

    long z = 0;

    for (int i = 0; i < parameters.Length; ++i)
    {
        var (p1, p2) = parameters[i];
        z = MysteriousOperation(GetNext(), z, p1, p2);
    }

    return z == 0;
}

(long lowest, long highest) = (long.MaxValue, long.MinValue);

/*Parallel.For(11111111111111, 99999999999999, i =>
{
    var s = i.ToString();
    if (Monad(s, monadParameters))
    {
        Console.WriteLine(s);
        //(lowest, highest) = (Math.Min(i, lowest), Math.Max(i, highest));
    }
});*/


// 43751151418198
//Console.WriteLine(lowest);
//Console.WriteLine(highest);

for (long i = 99999999999999; i >= 11111111111111; i--)
{
    var s = i.ToString();
    if (i % 1000000000000 == 0)
    {
        Console.WriteLine(s);
    }

    if (Monad(s, monadParameters))
    {
        Console.WriteLine(s);
    }
}


Console.ReadLine();

/*

Last operation needs these inputs (input digit, current value of z):
1, 10
2, 11
3, 12
4, 13
5, 14
6, 15
7, 16
8, 17
9, 18

*/

/*static List<(int, int)> FindPossibleInputs(int first, int second, int minZ, int maxZ, long? target)
{
    var result = new List<(int, int)>();
    for (int inputDigit = 9; inputDigit >= 1; inputDigit--)
    {
        for (int z = minZ; z <= maxZ; z++)
        {
            if (!target.HasValue || MysteriousOperation(inputDigit, z, first, second) == target)
            {
                result.Add((inputDigit, z));
                Console.WriteLine($"    {inputDigit}, {z}");
            }
        }
    }

    return result;
}


/*
static long FindLargestNumberForward(long z, IEnumerable<(int, int)> parameters, long acc)
{
    if (!parameters.Any())
    {
        if (z != 0)
        {
            return -1;
        }

        return acc;
    }

    var (first, second) = parameters.First();

    var result = -3L;

    for (int inputDigit = 9; inputDigit >= 1; inputDigit--)
    {
        var newZ = MysteriousOperation(inputDigit, z, first, second);

        if (newZ > 10000 || newZ < -10000)
        {
            continue;
        }

        result = Math.Max(result, FindLargestNumberForward(newZ, parameters.Skip(1), acc * 10 + inputDigit));
    }

    return result;
}

var (first, second) = parameters.First();

//foreach (var p in FindPossibleInputs(first, second, 0, 0, null))
//{
//    var inputDigit = p.Item1;
//    var z = p.Item2;
//    Console.WriteLine($"{inputDigit}, {z} => {MysteriousOperation(inputDigit, z, first, second)}:");

//    //foreach (var r in FindPossibleInputs)
//    //{

//    //}
//}
*/
/*var results = new List<(int, int)>[14];
var range = 10000;

for (int i = monadParameters.Length - 1; i >= 0; i--)
{
    results[i] = new List<(int, int)>(range);

    Console.WriteLine($"Doing {monadParameters[i]}");

    IEnumerable<int> possibleResults;
    if (i == monadParameters.Length - 1)
    {
        // Last operation needs to result in 0
        possibleResults = new[] { 0 };
    }
    else
    {
        possibleResults = results[i + 1].Select(p => p.Item2);
    }

    int r = range;

    while (results[i].Count == 0)
    {
        foreach (var possibleResult in possibleResults)
        {
            if (i == 0)
            {
                results[i].AddRange(FindPossibleInputs(monadParameters[i].Item1, monadParameters[i].Item2, 0, 0, possibleResult));
            }
            else
            {
                results[i].AddRange(FindPossibleInputs(monadParameters[i].Item1, monadParameters[i].Item2, 0 - r / 2, r / 2, possibleResult));
            }
        }

        r *= 10;

        if (results[i].Count == 0)
            Console.WriteLine($"  increasing range to {r}");
    }

    Console.WriteLine($"  has {results[i].Count}");
}

static long? FindLargestNumberForward2(long z, (int, int)[] parameters, int index, long acc)
{
    if (index == parameters.Length)
    {
        return z == 0 ? acc : null;
    }

    // Console.Write($"\r{acc}             ");

    // We first find possible different values for every digit (1..9)
    // Then we choose the largest number for this value
    // For example all digits could end up producing the same result => we only take 9
    var param1 = parameters[index].Item1;
    var param2 = parameters[index].Item2;
    var possibleValues = Enumerable.Range(1, 9)
        .Select(i => (i, MysteriousOperation(i, z, param1, param2)))
        .ToLookup(static pair => pair.Item2, static pair => pair.Item1)
        .Select(static group => (Z: group.Key, Digit: group.Max()))
        .OrderByDescending(p => p.Digit);

    return possibleValues.Select(p => FindLargestNumberForward2(p.Z, parameters, index + 1, acc * 10 + p.Digit)).FirstOrDefault(r => r is not null);
}

Console.WriteLine("Starting");

Console.WriteLine(FindLargestNumberForward2(0, monadParameters, 0, 0));


/*
static long FindLargestNumberBacktrack(long targetResult, IEnumerable<(int, int)> parameters, IEnumerable<char> acc)
{
    if (!parameters.Any())
    {
        return long.Parse(acc.Reverse().ToArray());
    }

    var (first, second) = parameters.First();

    var results = new List<long>
    {
        -1
    };

    for (int inputDigit = 9; inputDigit >= 1; inputDigit--)
    {

        for (int targetZ = 0; targetZ < 1000; targetZ++)
        {
            if (MysteriousOperation(inputDigit, targetZ, first, second) == targetResult)
            {
                results.Add(FindLargestNumber(targetZ, parameters.Skip(1), acc.Append((char)(inputDigit + '0'))));
            }
        }
    }

    return results.Max();
}*/

//Console.WriteLine(FindLargestNumber(0, parameters.Reverse(), Array.Empty<char>()));
