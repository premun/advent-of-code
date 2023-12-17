using AdventOfCode.Common;

namespace AdventOfCode._2021_04;

class Board(int[,] numbers)
{
    private const int Size = 5;
    private readonly int[,] _numbers = numbers;
    private bool[,] _crossed = new bool[Size, Size];

    public bool CrossNumber(int number)
    {
        foreach (var index in GetAllIndeces().Where(i => _numbers[i.Row, i.Column] == number))
        {
            _crossed[index.Row, index.Column] = true;
        }

        return IsWinning();
    }

    public int GetSumOfRemainingNumbers() => GetAllIndeces()
        .Where(index => !_crossed[index.Row, index.Column])
        .Select(index => _numbers[index.Row, index.Column])
        .Aggregate((acc, number) => acc + number);

    public void Reset()
    {
        _crossed = new bool[Size, Size];
    }

    private bool IsWinning()
        => Enumerable.Range(0, Size).Any(i => IsRowWinning(i) || IsColumnWinning(i));

    private bool IsRowWinning(int row)
        => !Enumerable.Range(0, Size).Any(column => !_crossed[row, column]);

    private bool IsColumnWinning(int column)
        => !Enumerable.Range(0, Size).Any(row => !_crossed[row, column]);

    private static IEnumerable<(int Row, int Column)> GetAllIndeces() =>
        Enumerable.Range(0, Size).AllCombinations();

    public static Board FromStream(StreamReader stream)
    {
        var numbers = new int[Size, Size];

        for (int row = 0; row < Size; row++)
        {
            var rowNumbers = stream.ReadLine()!
                .SplitBy(" ")
                .Select(int.Parse)
                .ToArray();

            for (int column = 0; column < Size; column++)
            {
                numbers[row, column] = rowNumbers[column];
            }
        }

        return new Board(numbers);
    }
}
