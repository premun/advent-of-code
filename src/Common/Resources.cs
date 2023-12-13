using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode.Common;

public static class Resources
{
    private const string DefaultInputName = "input.txt";
    private static readonly Regex NumberPattern = new(@"\-?\d+");

    public static string[] GetInputFileLines(string resourceFileName = DefaultInputName)
        => GetResourceFileContent(Assembly.GetCallingAssembly(), resourceFileName).SplitBy(Environment.NewLine);

    public static string GetInputFileContent(string resourceFileName = DefaultInputName)
        => GetResourceFileContent(Assembly.GetCallingAssembly(), resourceFileName);

    public static StreamReader GetResourceStream(string resourceFileName = DefaultInputName)
        => new(GetResourceStream(Assembly.GetCallingAssembly(), resourceFileName));

    private static string GetResourceFileContent(Assembly assembly, string resourceFileName = DefaultInputName)
    {
        using (var stream = GetResourceStream(assembly, resourceFileName))
        using (var sr = new StreamReader(stream ?? throw new FileNotFoundException($"Couldn't locate resource file '{resourceFileName}'")))
        {
            return sr.ReadToEnd();
        }
    }

    private static Stream GetResourceStream(Assembly assembly, string resourceFileName = DefaultInputName)
    {
        Stream? stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{resourceFileName}");

        if (stream == null)
        {
            string? resource = assembly.GetManifestResourceNames().FirstOrDefault(res => res.EndsWith(resourceFileName));
            if (resource != null)
            {
                stream = assembly.GetManifestResourceStream(resource);
            }
        }

        if (stream == null)
        {
            throw new Exception($"Resource {resourceFileName} not found in {assembly.GetName().Name}");
        }

        return stream;
    }

    public static int[] SplitToNumbers(this string csv) => csv
        .SplitBy(",")
        .Select(int.Parse)
        .ToArray();

    public static string[] SplitBy(this string s, string separator)
        => s.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    public static T[,] ParseAsArray<T>(this IEnumerable<string> input, Func<char, T> parser)
    {
        var list = input.ToList();
        var columnCount = list.First().Length;
        var result = new T[list.Count, columnCount];

        for (int row = 0; row < list.Count; row++)
        for (int column = 0; column < columnCount; column++)
        {
            result[row, column] = parser(list[row][column]);
        }

        return result;
    }

    public static T[][] ParseAsJaggedArray<T>(this IEnumerable<string> input, Func<char, T> parser)
        => input.Select(line => line.Select(c => parser(c)).ToArray()).ToArray();

    public static char[,] ParseAsArray(this IEnumerable<string> input)
        => ParseAsArray(input, c => c);

    public static char[][] ParseAsJaggedArray(this IEnumerable<string> input)
        => ParseAsJaggedArray(input, c => c);

    public static List<int> ParseNumbersOut(this string line)
        => NumberPattern.Matches(line)
            .Select(m => int.Parse(m.Value))
            .ToList();

    public static List<long> ParseLongNumbersOut(this string line)
        => NumberPattern.Matches(line)
            .Select(m => long.Parse(m.Value))
            .ToList();

    public static int BitsToInt(this IEnumerable<bool> bools, bool lowestBitFirst = true)
    {
        if (lowestBitFirst)
        {
            bools = bools.Reverse();
        }

        return bools
            .Select((b, index) => b ? 1 << index : 0)
            .Aggregate(0, (acc, v) => acc |= v);
    }

    public static IEnumerable<IEnumerable<T>> GroupsOf<T>(this IEnumerable<T> input, int groupSize)
    {
        var current = new T[groupSize];
        var index = 0;

        foreach (var item in input)
        {
            current[index++] = item;
            if (index != groupSize)
            {
                continue;
            }

            yield return current;

            current = new T[groupSize];
            index = 0;
        }

        if (index > 0)
        {
            yield return current.Take(index).ToArray();
        }
    }

    public static int Multiply(this IEnumerable<int> values)
        => values.Aggregate(1, (acc, v) => acc * v);

    public static long MultiplyAsLong(this IEnumerable<int> values)
        => values.Aggregate(1, (acc, v) => acc * v);

    public static long Multiply(this IEnumerable<long> values)
        => values.Aggregate(1L, (acc, v) => acc * v);

    public static long FindLowestCommonDenominator(this IEnumerable<int> numbers)
    {
        long lcm = numbers.First();
        foreach (int number in numbers.Skip(1))
        {
            lcm = GetLeastCommonMultiple(lcm, number);
        }

        return lcm;
    }

    public static long GetGreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static long GetLeastCommonMultiple(long a, long b)
    {
        return a * b / GetGreatestCommonDivisor(a, b);
    }
}
