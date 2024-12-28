using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

var machines = Resources.GetInputFileLines()
    .GroupsOf(3)
    .Select(lines => lines.Select(line => line.ParseNumbersOut()).ToArray())
    .Select(lines => new ClawMachine(
        new Coor(lines[0][1], lines[0][0]),
        new Coor(lines[1][1], lines[1][0]),
        new Coor(lines[2][1], lines[2][0])))
    .ToArray();

Console.WriteLine($"Part 1: {machines.Select(m => m.CalculateTokensToWinPrize()).Sum()}");
Console.WriteLine($"Part 2: {""}");

file record ClawMachine(Coor ButtonA, Coor ButtonB, Coor Prize)
{
    /*
        94a + 22b = 8400
        34a + 67b = 5400
        a < 100
        b < 100

        5400 * (94a + 22b) = 8400 * (34a + 67b)

        507600a + 118800b = 285600a + 562800b
        222000a = 444000b
        a = 2b
        94a + 11a = 8400
        105a = 8400
        a = 80
        b = 40
    */
    public int? CalculateTokensToWinPrize()
    {
        int a = ButtonA.X * Prize.Y - ButtonA.Y * Prize.X;
        int b = ButtonB.Y * Prize.X - ButtonB.X * Prize.Y;

        double ratio = (double)a / b;

        var aPresses = Prize.X / (ButtonA.X + ratio * ButtonB.X);
        var bPresses = aPresses * ratio;

        var intAPresses = (int)aPresses;
        var intBPresses = (int)bPresses;

        if (intAPresses > 100 || intBPresses > 100)
        {
            return 0;
        }

        if (Prize.X != intAPresses * ButtonA.X + intBPresses * ButtonB.X)
        {
            return 0;
            //throw new Exception("Somehow presses don't match");
        }

        if (Prize.Y != intAPresses * ButtonA.Y + intBPresses * ButtonB.Y)
        {
            return 0;
            //throw new Exception("Somehow presses don't match");
        }

        Console.WriteLine($"{intAPresses}, {intBPresses}");
        return intAPresses * 3 + intBPresses;
    }
}
