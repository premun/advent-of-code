namespace _18;

class SnailfishParser
{
    private readonly Stack<SnailfishNumber> _stack = new();

    public SnailfishNumber Parse(string input)
    {
        var reader = new TokenReader(input);
        _stack.Clear();

        while (true)
        {
            Token token = reader.GetNext(out var literal);

            switch (token)
            {
                case Token.Start:
                case Token.Delimeter:
                    break;

                case Token.Literal:
                    _stack.Push(new Literal(literal));
                    break;

                case Token.End:
                    var right = _stack.Pop();
                    var left = _stack.Pop();
                    var newNumber = new Pair(Right: right, Left: left);

                    left.Parent = newNumber;
                    right.Parent = newNumber;

                    newNumber.Reduce();

                    _stack.Push(newNumber);

                    break;

                case Token.EndOfStream:
                    if (_stack.Count != 1)
                    {
                        throw new InvalidOperationException($"Invalid count ({_stack.Count}) of token at the EOF!");
                    }

                    return _stack.Pop();

                default:
                    throw new Exception("???");
            }
        }

        throw new Exception("???");
    }
}
