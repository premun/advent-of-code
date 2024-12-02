namespace AdventOfCode.Common;

public static class DifferentCombinationsExtension
{
    public static IEnumerable<(T, T)> AllCombinations<T>(this IEnumerable<T> elements, bool includeIdentities = true, bool orderSensitive = false)
    {
        var query =
            from e1 in elements
            from e2 in elements
            where includeIdentities || !EqualityComparer<T>.Default.Equals(e1, e2)
            select (e1, e2);

        return orderSensitive
            ? query.SelectMany(pair => new[] { (pair.e1, pair.e2), (pair.e2, pair.e1) })
            : query;
    }

    public static int AddOrCreate<TKey>(this IDictionary<TKey, int> dict, TKey key, int valueToAdd, int defaultValue = 0)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, defaultValue);
        }

        dict[key] += valueToAdd;
        return dict[key];
    }

    public static long AddOrCreate<TKey>(this IDictionary<TKey, long> dict, TKey key, long valueToAdd, long defaultValue = 0)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, defaultValue);
        }

        dict[key] += valueToAdd;
        return dict[key];
    }

    public static IEnumerable<(T First, T Second)> PairWithNext<T>(this IEnumerable<T> elements)
    {
        var enumerator = elements.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            yield break;
        }

        var previous = enumerator.Current;
        while (enumerator.MoveNext())
        {
            yield return (previous, enumerator.Current);
            previous = enumerator.Current;
        }
    }
}
