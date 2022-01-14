namespace AdventOfCode._2021_19;

static class BipartityMatcher
{
    private static bool IsPerfectBipartityMatching(bool[,] graph, int u, bool[] seen, int[] matchings)
    {
        for (var v = 0; v < graph.GetLength(1); v++)
        {
            if (graph[u, v] && !seen[v])
            {
                seen[v] = true;

                if (matchings[v] < 0 || IsPerfectBipartityMatching(graph, matchings[v], seen, matchings))
                {
                    matchings[v] = u;
                    return true;
                }
            }
        }

        return false;
    }

    // Returns maximum matchings from M to N
    // Variation of Ford-Fulkerson algorithm for maximum flow problem
    public static IEnumerable<(int, int)> FindMaxBipartity(bool[,] graph)
    {
        var M = graph.GetLength(0);
        var N = graph.GetLength(1);

        // An array to keep track of the matchings
        var matchings = new int[N];

        for (var i = 0; i < N; ++i)
        {
            matchings[i] = -1;
        }

        var result = 0;
        for (var u = 0; u < M; u++)
        {
            var seen = new bool[N];
            for (var i = 0; i < N; ++i)
            {
                seen[i] = false;
            }

            if (IsPerfectBipartityMatching(graph, u, seen, matchings))
            {
                result++;
            }
        }

        return matchings.Select((m, n) => (m, n)).Where(x => x.m != -1);
    }
}
