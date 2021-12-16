using System.Diagnostics.CodeAnalysis;

namespace _16;

class BitReader
{
    private readonly string _source;
    private int _position = 0;

    public BitReader(string source)
    {
        _source = ToBits(source);
    }

    public bool TryReadBit([NotNullWhen(true)] out bool? bit)
    {
        if (_position >= _source.Length)
        {
            bit = null;
            return false;
        }

        bit = _source[_position++] == '1';

        return true;
    }

    public bool TryReadNumber(int numberOfBits, [NotNullWhen(true)] out int? number)
    {
        if (_position + numberOfBits >= _source.Length)
        {
            number = null;
            return false;
        }

        number = 0;
        int c = numberOfBits;

        // I am just trying out Span<T> and bit operations
        foreach (var b in _source.AsSpan(_position, numberOfBits))
        {
            c--;

            if (b == '1')
            {
                number |= (1 << c);
            }
        }

        _position += numberOfBits;

        return true;
    }

    public int ReadNumber(int numberOfBits)
    {
        if (!TryReadNumber(numberOfBits, out var number))
        {
            throw new InvalidDataException();
        }

        return number.Value;
    }

    public bool ReadBit()
    {
        if (!TryReadBit(out var bit))
        {
            throw new InvalidDataException();
        }

        return bit.Value;
    }

    public bool[] ReadBits(int numberOfBits)
    {
        var result = new bool[numberOfBits];

        for (int i = 0; i < numberOfBits; i++)
        {
            result[i] = _source[_position + i] == '1';
        }

        _position += numberOfBits;

        return result;
    }

    /// <summary>
    /// Turns D2FE28 into 110100101111111000101000
    /// </summary>
    private static string ToBits(string input) => string.Join(string.Empty, input
        .Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

    private static int ToInt(string binary) => Convert.ToInt32(binary, 2);
}
