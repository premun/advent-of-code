using System.Reflection;

namespace AdventOfCode.Common;

public static class Resources
{
    private const string DefaultInputName = "input.txt";

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

    public static char[,] ParseAsArray(this IEnumerable<string> input) => ParseAsArray(input, c => c);

    public static char[][] ParseAsJaggedArray(this IEnumerable<string> input) => ParseAsJaggedArray(input, c => c);

    public static IEnumerable<IGrouping<int, T>> GroupsOf<T>(this IEnumerable<T> input, int groupSize) => input
        .Select((item, index) => (Group: index / groupSize, Items: item))
        .GroupBy(k => k.Group, v => v.Items);
}
