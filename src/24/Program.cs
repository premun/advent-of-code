static long MysteriousOperation(long input, long z, int first, int second)
{
    long x = (z % 26) + first;

    if (first < 0)
    {
        z /= 26;
    }

    if (x != input)
    {
        z *= 26;
        z += input + second;
    }

    return z;
}

static bool Monad(string input)
{
    if (input.Contains('0'))
    {
        return false;
    }

    var position = 0;
    long GetNext()
    {
        return input[position++] - '0';
    }

    long z = 0;

    z = MysteriousOperation(GetNext(), z, 15, 13);
    z = MysteriousOperation(GetNext(), z, 10, 16);
    z = MysteriousOperation(GetNext(), z, 12, 2);
    z = MysteriousOperation(GetNext(), z, 10, 8);
    z = MysteriousOperation(GetNext(), z, 14, 11);
    z = MysteriousOperation(GetNext(), z, -11, 6);
    z = MysteriousOperation(GetNext(), z, 10, 12);
    z = MysteriousOperation(GetNext(), z, -16, 2);
    z = MysteriousOperation(GetNext(), z, -9, 2);
    z = MysteriousOperation(GetNext(), z, 11, 15);
    z = MysteriousOperation(GetNext(), z, -8, 1);
    z = MysteriousOperation(GetNext(), z, -8, 10);
    z = MysteriousOperation(GetNext(), z, -10, 14);
    z = MysteriousOperation(GetNext(), z, -9, 10);

    return z == 0;
}

var parameters = new[]
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

static List<(int, int)> FindPossibleInputs(int first, int second, int minZ, int maxZ, long? target)
{
    var result = new List<(int, int)>();
    for (int inputDigit = 9; inputDigit >= 1; inputDigit--)
    {
        for (int z = minZ; z <= maxZ; z++)
        {
            if (!target.HasValue || MysteriousOperation(inputDigit, z, first, second) == target)
            {
                result.Add((inputDigit, z));
                //Console.WriteLine($"    {inputDigit}, {z}");
            }
        }
    }

    return result;
}

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

var results = new List<(int, int)>[14];
var range = 100;

for (int i = parameters.Length - 1; i >= 0; i--)
{
    results[i] = new List<(int, int)>(range);

    Console.WriteLine($"Doing {parameters[i]}");

    IEnumerable<int> possibleResults;
    if (i == parameters.Length - 1)
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
                results[i].AddRange(FindPossibleInputs(parameters[i].Item1, parameters[i].Item2, 0, 0, possibleResult));
            }
            else
            {
                results[i].AddRange(FindPossibleInputs(parameters[i].Item1, parameters[i].Item2, 0 - r / 2, r / 2, possibleResult));
            }
        }

        r *= 10;

        if (results[i].Count == 0)
            Console.WriteLine($"  increasing range to {r}");
    }

    Console.WriteLine($"  has {results[i].Count}");
}

for (int i = 0; i < results.Length; i++)
{

}

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
