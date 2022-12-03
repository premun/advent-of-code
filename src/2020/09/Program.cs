using AdventOfCode.Common;

var numbers = Resources.GetInputFileLines()
    .Select(long.Parse)
    .ToArray();

static bool IsValid(LinkedList<long> buffer, long number)
{
    var first = buffer.First;
    while (first != null)
    {
        var second = first.Next;
        while (second != null)
        {
            if (first.Value + second.Value == number)
            {
                return true;
            }

            second = second.Next;
        }

        first = first.Next;
    }

    return false;
}

static long FindInvalidNumber(IEnumerable<long> numbers, int preambleSize)
{
    var buffer = new LinkedList<long>(numbers.Take(preambleSize));

    foreach (var number in numbers.Skip(preambleSize))
    {
        if (!IsValid(buffer, number))
        {
            return number;
        }

        buffer.RemoveFirst();
        buffer.AddLast(number);
    }

    throw new Exception("No invalid number");
}

static IEnumerable<long> FindSequenceWithSum(long[] numbers, long sum)
{
    // sums[x, y] is a sum of a sequence ending at y of length x
    var sums = new long[numbers.Length + 1, numbers.Length];

    for (int length = 1; length <= numbers.Length; length++)
    {
        for (var end = length - 1; end < numbers.Length; end++)
        {
            if (length == 1)
            {
                sums[length, end] = numbers[end];
                continue;
            }

            sums[length, end] = sums[length - 1, end] + numbers[end - length + 1];

            if (sums[length, end] == sum)
            {
                return numbers.Skip(end - length + 1).Take(length);
            }
        }
    }

    throw new Exception("No sequence has this sum");
}

var invalidNumber = FindInvalidNumber(numbers, 25);
var sequence = FindSequenceWithSum(numbers, invalidNumber);

Console.WriteLine($"Part 1: {invalidNumber}");
Console.WriteLine($"Part 2: {sequence.Min() + sequence.Max()}");
