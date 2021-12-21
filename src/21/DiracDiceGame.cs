namespace _21;

class Player
{
    public int Position { get; set; }
    public int Points { get; set; }
}

class DiracDiceGame
{
    private readonly IDice _dice;
    private readonly int _maxPosition;
    private readonly int _winningPoints;
    private readonly Player[] _players;
    private int _currentPlayer = 0;

    public DiracDiceGame(IDice dice, int[] startingPosition, int maxPosition = 10, int winningPoints = 1000)
    {
        _dice = dice;
        _maxPosition = maxPosition;
        _winningPoints = winningPoints;
        _players = startingPosition
            .Select(position => new Player()
            {
                Position = position - 1,
                Points = 0,
            })
            .ToArray();
    }

    public Player[] RunGame()
    {
        Player player;

        do
        {
            player = _players[_currentPlayer];

            var points = _dice.Throw() + _dice.Throw() + _dice.Throw();
            player.Position += points;
            player.Position %= _maxPosition;
            player.Points += player.Position + 1;
            _currentPlayer++;
            _currentPlayer %= _players.Length;

        } while (player.Points < _winningPoints);

        return _players
            .Select(p => new Player
            {
                Position = p.Position == _maxPosition ? 1 : p.Position + 1,
                Points = p.Points,
            })
            .ToArray();
    }
}
