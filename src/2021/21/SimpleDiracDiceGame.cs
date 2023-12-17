namespace AdventOfCode._2021_21;

class Player
{
    public int Position { get; set; }
    public int Points { get; set; }
}

class SimpleDiracDiceGame(IDice dice, int maxPosition, int winningPoints) : DiracDiceGame(maxPosition)
{
    private readonly IDice _dice = dice;
    private readonly int _winningPoints = winningPoints;
    private int _currentPlayer = 0;

    public Player[] RunGame(int[] startingPosition)
    {
        var players = startingPosition
            .Select(position => new Player()
            {
                Position = position - 1,
                Points = 0,
            })
            .ToArray();

        Player player;

        do
        {
            player = players[_currentPlayer];

            var diceRoll = _dice.Throw() + _dice.Throw() + _dice.Throw();

            (player.Position, player.Points) = MovePlayer(player.Position, player.Points, diceRoll);

            _currentPlayer++;
            _currentPlayer %= players.Length;

        } while (player.Points < _winningPoints);

        return players
            .Select(p => new Player
            {
                Position = p.Position + 1,
                Points = p.Points,
            })
            .ToArray();
    }
}
