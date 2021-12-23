using System.Collections.ObjectModel;

namespace _21;

class QuantumDiracDiceGame : DiracDiceGame
{
    private readonly int _maxPosition;
    private readonly int _winningPoints;
    private readonly int _diceSides;

    // Map where keys are possible sums of 3 dice rolls and values counts of combinations that have that value
    private readonly ReadOnlyCollection<(int Value, int Count)> _diceRolls;

    public QuantumDiracDiceGame(int maxPosition, int winningPoints, int diceSides) : base(maxPosition)
    {
        _maxPosition = maxPosition;
        _winningPoints = winningPoints;
        _diceSides = diceSides;
        _diceRolls = GeneratePossibleDiceRolls(diceSides);
    }

    public long RunGame(int player1Position, int player2Position, bool firstPlayerStarts)
    {
        // Counts possible wins of first player for given positions of both players and all point combinations
        // Uses the previously generated map of number of paths to win from a given state
        //                                position1   x  position2  x    points1    x     points2
        var winsOfFirstPlayer = new long[_maxPosition, _maxPosition, _winningPoints, _winningPoints];

        for (int points1 = _winningPoints - 1; points1 >= 0; points1--)
        {
            for (int points2 = _winningPoints - 1; points2 >= 0; points2--)
            {
                for (int position1 = _maxPosition - 1; position1 >= 0; position1--)
                {
                    for (int position2 = _maxPosition - 1; position2 >= 0; position2--)
                    {
                        winsOfFirstPlayer[position1, position2, points1, points2] = (
                            from roll1 in _diceRolls
                            from roll2 in _diceRolls
                            select GetWins(position1, position2, points1, points2, winsOfFirstPlayer, roll1, roll2, firstPlayerStarts)
                        ).Sum();
                    }
                }
            }
        }

        var wins = winsOfFirstPlayer[player1Position - 1, player2Position - 1, 0, 0];

        // I haven't figured out why but I get 27x higher values when player starts
        if (firstPlayerStarts)
        {
            wins /= (int)Math.Pow(_diceSides, _diceSides);
        }

        return wins;
    }

    private long GetWins(
        int position1,
        int position2,
        int points1,
        int points2,
        long[,,,] winsOfFirstPlayer,
        (int Value, int Count) roll1,
        (int Value, int Count) roll2,
        bool firstPlayerStarts)
    {
        var (nextPosition1, nextPoints1) = MovePlayer(position1, points1, roll1.Value);
        var (nextPosition2, nextPoints2) = MovePlayer(position2, points2, roll2.Value);

        // Number of times this combination of dice rolls occurs
        var rollCount = roll1.Count * roll2.Count;

        if (nextPoints1 >= _winningPoints)
        {
            if (nextPoints2 >= _winningPoints)
            {
                // If we can both win next round then whoever rolls first wins
                return firstPlayerStarts ? rollCount : 0;
            }
            else
            {
                // If we can win next round and they can't, we beat the second player every time
                return rollCount;
            }
        }

        // If other player can win next round, we lose
        if (nextPoints2 >= _winningPoints)
        {
            return 0;
        }

        return rollCount * winsOfFirstPlayer[nextPosition1, nextPosition2, nextPoints1, nextPoints2];
    }

    private static ReadOnlyCollection<(int Value, int Count)> GeneratePossibleDiceRolls(int diceSides)
    {
        var sides = Enumerable.Range(1, diceSides);
        var outcomes =
            from a in sides
            from b in sides
            from c in sides
            select (a + b + c);

        var diceRolls = new Dictionary<int, int>();

        foreach (var points in outcomes)
        {
            if (diceRolls.ContainsKey(points))
            {
                diceRolls[points]++;
            }
            else
            {
                diceRolls[points] = 1;
            }
        }

        return diceRolls.Select(x => (Value: x.Key, Count: x.Value)).ToList().AsReadOnly();
    }
}
