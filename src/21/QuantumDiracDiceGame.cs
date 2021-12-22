using Common;

namespace _21;

class QuantumDiracDiceGame
{
    private readonly int _maxPosition;
    private readonly int _winningPoints;
    private readonly Dictionary<int, int> _diceRolls;

    // For every state (position on map + point count) holds a map of (num of turns) => (number of wins after that many turns)
    // So for every state, we know how many turns we can get to how many different wins
    private readonly Dictionary<int, long>[,] _pathMap;

    public QuantumDiracDiceGame(int maxPosition, int winningPoints, int diceSides)
    {
        _maxPosition = maxPosition;
        _winningPoints = winningPoints;

        var sides = Enumerable.Range(1, diceSides);
        var outcomes =
            from a in sides
            from b in sides
            from c in sides
            select (a + b + c);

        _diceRolls = new Dictionary<int, int>();

        foreach (var points in outcomes)
        {
            if (_diceRolls.ContainsKey(points))
            {
                _diceRolls[points]++;
            }
            else
            {
                _diceRolls[points] = 1;
            }
        }

        _pathMap = GetPathMap();
    }

    public long RunGame(int player1Position, int player2Position, bool firstPlayerStarts)
    {
        // Counts possible wins of first player for given positions of both players and all point combinations
        // Uses the previously generated map of number of paths to win from a given state
        var winsOfFirstPlayer = new long[_maxPosition, _maxPosition, _winningPoints, _winningPoints];

        for (int points1 = _winningPoints - 1; points1 >= 0; points1--)
        {
            for (int points2 = _winningPoints - 1; points2 >= 0; points2--)
            {
                for (int position1 = _maxPosition - 1; position1 >= 0; position1--)
                {
                    for (int position2 = _maxPosition - 1; position2 >= 0; position2--)
                    {
                        foreach (var outcome in _diceRolls)
                        {
                            var diceRoll = outcome.Key;
                            var rollCount = outcome.Value;
                            var nextPosition1 = (position1 + diceRoll) % _maxPosition;
                            var nextPosition2 = (position2 + diceRoll) % _maxPosition;
                            var nextPoints1 = points1 + nextPosition1 + 1;
                            var nextPoints2 = points2 + nextPosition2 + 1;

                            if (nextPoints1 >= _winningPoints)
                            {
                                if (nextPoints2 >= _winningPoints)
                                {
                                    if (firstPlayerStarts)
                                    {
                                        // If we can both win next round then whoever rolls first wins
                                        winsOfFirstPlayer[position1, position2, points1, points2] += rollCount;
                                    }
                                }
                                else
                                {
                                    // If we can win next round and they can't, we beat the second player every time
                                    winsOfFirstPlayer[position1, position2, points1, points2] += rollCount;
                                }

                                continue;
                            }

                            // If other player can win next round, we lose
                            if (nextPoints2 >= _winningPoints)
                            {
                                continue;
                            }

                            winsOfFirstPlayer[position1, position2, points1, points2] +=
                                GetWins(position1, position2, points1, points2, firstPlayerStarts) * rollCount;
                        }
                    }
                }
            }
        }

        return Math.Max(winsOfFirstPlayer[player1Position - 1, 0, 0, 0], winsOfFirstPlayer[player2Position - 1, 0, 0, 0]);
    }

    // For given game state (both player position + their points), returns the number of shorter paths from first position
    private long GetWins(int position1, int position2, int points1, int points2, bool firstStarts) =>
    (
        from m1 in _pathMap[position1, points1]
        from m2 in _pathMap[position2, points2]
        let turnsToWin1 = m1.Key
        let turnsToWin2 = m2.Key
        let pathCount = m1.Value
        where (turnsToWin1 < turnsToWin2) || (firstStarts && turnsToWin1 == turnsToWin2)
        select m1.Value
    ).Sum();

    // For every state (position on map + point count) holds a map of (num of turns) => (number of wins after that many turns)
    // So for every state, we know how many turns we can get to how many different wins
    // Calculates it from the back and uses previous results (DP style).
    private Dictionary<int, long>[,] GetPathMap()
    {
        var possibleGames = new Dictionary<int, long>[_maxPosition, _winningPoints];

        for (int points = _winningPoints - 1; points >= 0; points--)
        {
            for (int position = _maxPosition - 1; position >= 0; position--)
            {
                var current = new Dictionary<int, long>();

                foreach (var outcome in _diceRolls)
                {
                    var diceRoll = outcome.Key;
                    var rollCount = outcome.Value;
                    var nextPosition = (position + diceRoll) % _maxPosition;
                    var nextPoints = points + nextPosition + 1;

                    if (nextPoints >= _winningPoints)
                    {
                        // Next roll will get us a win
                        current.AddOrCreate(1, rollCount);
                    }
                    else
                    {
                        // Next roll will bring us to some next state
                        var next = possibleGames[nextPosition, nextPoints];
                        foreach (var pair in next)
                        {
                            // Add the new roll + add the new count
                            current.AddOrCreate(pair.Key + 1, pair.Value * rollCount);
                        }
                    }
                }

                possibleGames[position, points] = current;
            }
        }

        return possibleGames;
    }
}
