using System.Collections.ObjectModel;
using Common;

namespace _21;

class QuantumDiracDiceGame : DiracDiceGame
{
    private readonly int _maxPosition;
    private readonly int _winningPoints;

    // Map where keys are possible sums of 3 dice rolls and values counts of combinations that have that value
    private readonly ReadOnlyCollection<(int Value, int Count)> _diceRolls;

    // For every state (position on map + point count) holds a map of (num of turns) => (number of wins after that many turns)
    // So for every state, we know how many turns we can get to how many different wins
    private readonly Dictionary<int, long>[,] _pathMap;

    public QuantumDiracDiceGame(int maxPosition, int winningPoints, int diceSides) : base(maxPosition)
    {
        _maxPosition = maxPosition;
        _winningPoints = winningPoints;

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

        _diceRolls = diceRolls.Select(x => (Value: x.Key, Count: x.Value)).ToList().AsReadOnly();
        _pathMap = GetPathMap();
    }

    public long RunGame(int player1Position, int player2Position, bool firstPlayerStarts)
    {
        // Counts possible wins of first player for given positions of both players and all point combinations
        // Uses the previously generated map of number of paths to win from a given state
        //                                position1   x  position2  x   points1    x     points2
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
                            select GetWins(
                                position1,
                                position2,
                                points1,
                                points2,
                                winsOfFirstPlayer,
                                roll1,
                                roll2,
                                firstPlayerStarts)
                        ).Sum()/* + GetWins(position1, position2, points1, points2, firstPlayerStarts)*/;
                    }
                }
            }
        }

        return winsOfFirstPlayer[player1Position - 1, player2Position - 1, 0, 0];
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
                return firstPlayerStarts ? 1 : 0;
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

    // For given game state (both player position + their points), returns the number of shorter paths from first position
    private long GetWins(int position1, int position2, int points1, int points2, bool firstStarts) =>
    (
        from m1 in _pathMap[position1, points1]
        from m2 in _pathMap[position2, points2]
        let turnsToWin1 = m1.Key
        let turnsToWin2 = m2.Key
        let pathCount = m1.Value
        where (turnsToWin1 < turnsToWin2) || (firstStarts && turnsToWin1 == turnsToWin2)
        select pathCount
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

                foreach (var roll in _diceRolls)
                {
                    var (nextPosition, nextPoints) = MovePlayer(position, points, roll.Value);

                    if (nextPoints >= _winningPoints)
                    {
                        // Next roll will get us a win
                        current.AddOrCreate(1, roll.Count);
                    }
                    else
                    {
                        // Next roll will bring us to some next state
                        var next = possibleGames[nextPosition, nextPoints];
                        foreach (var pair in next)
                        {
                            // Add the new roll + add the new count
                            current.AddOrCreate(pair.Key + 1, pair.Value * roll.Count);
                        }
                    }
                }

                possibleGames[position, points] = current;
            }
        }

        return possibleGames;
    }
}
