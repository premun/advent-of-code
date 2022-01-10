using System.Reflection;

namespace AdventOfCode.Common;

public static class Resources
{
    public static string[] GetResourceFileLines(string resourceFileName)
        => GetResourceFile(Assembly.GetCallingAssembly(), resourceFileName).SplitBy(Environment.NewLine);

    public static string GetResourceFile(string resourceFileName)
        => GetResourceFile(Assembly.GetCallingAssembly(), resourceFileName);

    public static StreamReader GetResourceStream(string resourceFileName)
        => new(GetResourceStream(Assembly.GetCallingAssembly(), resourceFileName));

    private static string GetResourceFile(Assembly assembly, string resourceFileName)
    {
        using (var stream = GetResourceStream(assembly, resourceFileName))
        using (var sr = new StreamReader(stream ?? throw new FileNotFoundException($"Couldn't locate resource file '{resourceFileName}'")))
        {
            return sr.ReadToEnd();
        }
    }

    private static Stream GetResourceStream(Assembly assembly, string resourceFileName)
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
        {
            for (int column = 0; column < columnCount; column++)
            {
                result[row, column] = parser(list[row][column]);
            }
        }

        return result;
    }

    public static T[][] ParseAsJaggedArray<T>(this IEnumerable<string> input, Func<char, T> parser)
        => input.Select(line => line.Select(c => parser(c)).ToArray()).ToArray();

    public static char[,] ParseAsArray(this IEnumerable<string> input) => ParseAsArray(input, c => c);

    public static char[][] ParseAsJaggedArray(this IEnumerable<string> input) => ParseAsJaggedArray(input, c => c);
}
