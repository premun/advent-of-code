namespace AdventOfCode._2021_21;

abstract class DiracDiceGame(int maxPosition)
{
    protected (int Position, int Points) MovePlayer(int position, int points, int diceRoll)
    {
        var nextPosition = (position + diceRoll) % maxPosition;
        return (nextPosition, points + nextPosition + 1);
    }
}
