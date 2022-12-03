using AdventOfCode.Common;

var startingNumbers = Resources.GetInputFileContent().SplitToNumbers();

static int GetNumber(int[] startingNumbers, int index)
{
    var numbers = startingNumbers.Select((n, index) => (n, index)).ToDictionary(t => t.n, t => t.index + 1);

    var lastIndexOf = Array.LastIndexOf(startingNumbers, startingNumbers.Last(), startingNumbers.Length - 2, startingNumbers.Length - 1);
    var current = lastIndexOf > -1 && lastIndexOf < startingNumbers.Length - 1 ? startingNumbers.Length - lastIndexOf : 0;

    for (int i = startingNumbers.Length + 1; i < index; i++)
    {
        var c = current;

        if (numbers.TryGetValue(current, out var prevAppearance))
        {
            current = i - prevAppearance;
        }
        else
        {
            current = 0;
        }

        numbers[c] = i;
    }

    return current;
}


Console.WriteLine(GetNumber(startingNumbers, 2020));
Console.WriteLine(GetNumber(startingNumbers, 30000000));
