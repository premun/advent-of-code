namespace _21;

class Player
{
    public int Position { get; set; }
    public int Points { get; set; }
}

class SimpleDiracDiceGame
{
    private readonly IDice _dice;
    private readonly int _maxPosition;
    private readonly int _winningPoints;
    private int _currentPlayer = 0;

    public SimpleDiracDiceGame(IDice dice, int maxPosition, int winningPoints)
    {
        _dice = dice;
        _maxPosition = maxPosition;
        _winningPoints = winningPoints;
    }

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

            var points = _dice.Throw() + _dice.Throw() + _dice.Throw();
            player.Position += points;
            player.Position %= _maxPosition;
            player.Points += player.Position + 1;
            _currentPlayer++;
            _currentPlayer %= players.Length;

        } while (player.Points < _winningPoints);

        return players
            .Select(p => new Player
            {
                Position = p.Position == _maxPosition ? 1 : p.Position + 1,
                Points = p.Points,
            })
            .ToArray();
    }
}
