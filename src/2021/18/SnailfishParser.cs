namespace AdventOfCode._2021_18;

static class SnailfishParser
{
    public static SnailfishNumber Parse(string input)
    {
        var reader = new TokenReader(input);
        var stack = new Stack<SnailfishNumber>();

        while (true)
        {
            Token token = reader.GetNext(out var literal);

            switch (token)
            {
                case Token.Start:
                case Token.Delimeter:
                    break;

                case Token.Literal:
                    stack.Push(new Literal(literal));
                    break;

                case Token.End:
                    var right = stack.Pop();
                    var left = stack.Pop();

                    var newNumber = new Pair(right: right, left: left);
                    newNumber.Reduce();

                    stack.Push(newNumber);

                    break;

                case Token.EndOfStream:
                    if (stack.Count != 1)
                    {
                        throw new InvalidOperationException($"Invalid count ({stack.Count}) of token at the EOF!");
                    }

                    return stack.Pop();

                default:
                    throw new Exception("???");
            }
        }

        throw new Exception("???");
    }
}
