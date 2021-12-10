namespace _10;

class CorruptedLineException : Exception
{
    public CorruptedLineException(char c)
    {
        CorruptedChar = c;
    }

    public char CorruptedChar { get; }
}
