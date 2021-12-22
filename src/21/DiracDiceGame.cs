namespace _21;

abstract class DiracDiceGame
{
    private readonly int _maxPosition;

    protected DiracDiceGame(int maxPosition)
    {
        _maxPosition = maxPosition;
    }

    protected (int Position, int Points) MovePlayer(int position, int points, int diceRoll)
    {
        var nextPosition = (position + diceRoll) % _maxPosition;
        return (nextPosition, points + nextPosition + 1);
    }
}
