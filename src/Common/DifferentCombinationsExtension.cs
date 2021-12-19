namespace Common;

public static class DifferentCombinationsExtension
{
    public static IEnumerable<(T, T)> AllCombinations<T>(this IEnumerable<T> elements, bool includeIdentities = true, bool orderSensitive = false)
    {
        if (orderSensitive)
        {
            return elements
                .SelectMany(e => elements.Where(f => includeIdentities || !EqualityComparer<T>.Default.Equals(e, f))
                .SelectMany(f => new[] { (e, f), (f, e) }));
        }
        else
        {
            return elements
                .SelectMany(e => elements.Where(f => includeIdentities || !EqualityComparer<T>.Default.Equals(e, f))
                .Select(f => (e, f) ));
        }
    }
}
