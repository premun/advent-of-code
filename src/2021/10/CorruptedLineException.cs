namespace AdventOfCode._2021_10;

class CorruptedLineException(char c) : Exception
{
    public char CorruptedChar { get; } = c;
}
