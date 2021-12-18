namespace _18;

enum Token
{
    Start,      // [
    Literal,    // number
    End,        // ]
    Delimeter,  // ,
    EndOfStream,
}

class TokenReader
{
    private readonly string _source;

    public int Position { get; private set; } = 0;

    public TokenReader(string source)
    {
        _source = source;
    }

    public Token GetNext(out long literalValue)
    {
        literalValue = 0;

        if (Position == _source.Length)
        {
            return Token.EndOfStream;
        }

        switch (Current)
        {
            case '[':
                Position++;
                return Token.Start;

            case ']':
                Position++;
                return Token.End;

            case ',':
                Position++;
                return Token.Delimeter;

            default:
                while (Position < _source.Length && Current >= '0' && Current <= '9')
                {
                    literalValue *= 10;
                    literalValue += Current - '0';
                    Position++;
                }

                return Token.Literal;
        }
    }

    private char Current => _source[Position];
}
