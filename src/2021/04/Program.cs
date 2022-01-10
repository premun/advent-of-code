using AdventOfCode._2021_04;
using AdventOfCode.Common;

using var inputReader = Resources.GetResourceStream("input.txt");

var drawnNumbers = inputReader.ReadLine()!.SplitToNumbers();

inputReader.ReadLine();

var boards = new List<Board>();

while (!inputReader.EndOfStream)
{
    boards.Add(Board.FromStream(inputReader));
    inputReader.ReadLine();
}

static int GetFirstWinningBoardScore(IEnumerable<int> drawnNumbers, IEnumerable<Board> boards)
{
    foreach (var number in drawnNumbers)
    {
        var winningBoard = boards.FirstOrDefault(board => board.CrossNumber(number));
        if (winningBoard != null)
        {
            return number * winningBoard.GetSumOfRemainingNumbers();
        }
    }

    throw new Exception("No winning board");
}

static int GetLastWinningBoardScore(IEnumerable<int> drawnNumbers, IEnumerable<Board> boards)
{
    var currentRound = new Queue<Board>(boards);
    var nextRound = new Queue<Board>();
    Board? lastWinningBoard = null;

    foreach (var number in drawnNumbers)
    {
        while (currentRound.Any())
        {
            var board = currentRound.Dequeue();

            if (!board.CrossNumber(number))
            {
                nextRound.Enqueue(board);
            }
            else
            {
                lastWinningBoard = board;
            }
        }

        if (!nextRound.Any())
        {
            return number * (lastWinningBoard ?? throw new Exception("No winning board")).GetSumOfRemainingNumbers();
        }

        currentRound = nextRound;
        nextRound = new();
    }

    throw new Exception("Multiple winning boards");
}

Console.WriteLine($"Part 1: {GetFirstWinningBoardScore(drawnNumbers, boards)}");

foreach (var board in boards)
{
    board.Reset();
}

Console.WriteLine($"Part 2: {GetLastWinningBoardScore(drawnNumbers, boards)}");
