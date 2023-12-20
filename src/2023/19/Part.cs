using AdventOfCode.Common;

record Part(int X, int M, int A, int S)
{
    public static Part FromDefinition(string definition)
    {
        var numbers = definition.ParseNumbersOut();
        return new Part(numbers[0], numbers[1], numbers[2], numbers[3]);
    }
}
