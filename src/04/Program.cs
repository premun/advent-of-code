using _04;
using Common;

using var inputReader = Resources.GetResourceStream("input.txt");

var drawnNumbers = inputReader.ReadLine()!.Split(",").Select(int.Parse);

inputReader.ReadLine();

var boards = new List<Board>();

while (!inputReader.EndOfStream)
{
    boards.Add(Board.FromStream(inputReader));
    inputReader.ReadLine();
}

int Part1()
{
    foreach (var number in drawnNumbers)
    {
        foreach (var board in boards)
        {
            if (board.CrossNumber(number))
            {
                return number * board.GetSumOfRemainingNumbers();
            }
        }
    }

    throw new Exception("No winning board");
}

Console.WriteLine($"Part 1: {Part1()}");
