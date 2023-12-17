using AdventOfCode.Common;
using Common;

char[,] map = Resources.GetInputFileLines().ParseAsArray();
var symbols = new List<Symbol>();
var numbers = new List<Number>();

for (int row = 0; row < map.Width(); row++)
{
    for (int col = 0; col < map.Height(); col++)
    {
        var c = map[row, col];
        switch (c)
        {
            case '.':
                break;

            case >= '0' and <= '9':
                var number = new Number(c - '0', row, col, col);
                while (++col < map.Height() && char.IsDigit(map[row, col]))
                {
                    number = new Number(number.Value * 10 + map[row, col] - '0', number.Row, number.Col, col);
                }

                --col;
                numbers.Add(number);
                break;

            default:
                symbols.Add(new Symbol(c, row, col));
                break;
        }
    }
}

IEnumerable<int> adjacentParts = numbers
    .Where(number => symbols.Any(symbol => AreAdjacent(number, symbol)))
    .Select(p => p.Value);

IEnumerable<long> gearScores = symbols
    .Select(symbol => numbers
        .Where(number => AreAdjacent(number, symbol)).ToList())
    .Where(adjacent => adjacent.Count == 2)
    .Select(adjacent => adjacent.Aggregate(1L, (a, p) => a * p.Value));

Console.WriteLine($"Part 1: {adjacentParts.Sum()}");
Console.WriteLine($"Part 2: {gearScores.Sum()}");

static bool AreAdjacent(Number number, Symbol symbol)
{
    // Row around
    if (!Around(number.Row, symbol.Row)) return false;

    // Contains
    if (number.Col <= symbol.Col && number.EndCol >= symbol.Col) return true;

    // Starts/ends near
    if (Around(number.Col, symbol.Col) || Around(number.EndCol, symbol.Col)) return true;

    return false;
}

static bool Around(int x, int y) => Math.Abs(y - x) <= 1;

file record Symbol(char Sign, int Row, int Col);
file record Number(int Value, int Row, int Col, int EndCol);
