﻿namespace AdventOfCode._2021_18;

enum Token
{
    Start,      // [
    Literal,    // number
    End,        // ]
    Delimeter,  // ,
    EndOfStream,
}

class TokenReader(string source)
{
    private readonly string _source = source;

    public int Position { get; private set; } = 0;

    public bool EndOfStream => Position == _source.Length;

    public Token GetNext(out long literalValue)
    {
        literalValue = 0;

        if (EndOfStream)
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
                while (!EndOfStream && char.IsNumber(Current))
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
