// This operation is being repeated for every digit
static long MysteriousOperation(int input, long z, (int, int) parameters)
{
    long x = z % 26;

    if (parameters.Item1 < 0)
    {
        z /= 26;
    }

    // This is the if we are trying to avoid for negavit first parameter
    if (x != input - parameters.Item1)
    {
        z *= 26;
        z += input + parameters.Item2;
    }

    return z;
}

// This is the MONAD that the submarine would actually run
static bool Monad(string input, (int, int)[] parameters)
{
    if (input.Contains('0'))
    {
        return false;
    }

    var position = 0;
    int GetNext()
    {
        return input[position++] - '0';
    }

    long z = 0;

    for (int i = 0; i < parameters.Length; ++i)
    {
        z = MysteriousOperation(GetNext(), z, parameters[i]);
    }

    return z == 0;
}

static long? FindModelNumberRecursive(long z, (int, int)[] parameters, int index, long acc, bool highestNumber)
{
    if (index == parameters.Length)
    {
        return z == 0 ? acc : null;
    }

    // Based on highest/lowest flag, either run for 1..9 or 9..1
    for (int i = highestNumber ? 9 : 1; (highestNumber && i > 0) || (!highestNumber && i <= 9); i += (highestNumber ? -1 : 1))
    {
        var p = parameters[index];

        // When z can go down, let's make sure it does
        // When first parameters is < 0, z is divided so let's make sure we don't hit the if() that multiplies z later
        if (p.Item1 > 0 || z % 26 == i - p.Item1)
        {
            var newZ = MysteriousOperation(i, z, p);
            var result = FindModelNumberRecursive(newZ, parameters, index + 1, acc * 10 + i, highestNumber);

            if (result != null)
            {
                // This is an extra check but why not..
                if (!Monad(result.ToString()!, parameters))
                {
                    throw new Exception("???");
                }

                return result;
            }
        }
    }

    return null;
}

static long? FindModelNumber(bool highestNumber)
{
    // This comes from the input
    var monadParameters = new[]
    {
        (15, 13),
        (10, 16),
        (12, 2),
        (10, 8),
        (14, 11),
        (-11, 6),
        (10, 12),
        (-16, 2),
        (-9, 2),
        (11, 15),
        (-8, 1),
        (-8, 10),
        (-10, 14),
        (-9, 10),
    };

    return FindModelNumberRecursive(0, monadParameters, 0, 0, highestNumber);
}

Console.WriteLine("Part 1: " + FindModelNumber(true));
Console.WriteLine("Part 2: " + FindModelNumber(false));
