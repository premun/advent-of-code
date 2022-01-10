namespace AdventOfCode._2021_21;

interface IDice
{
    int Throws { get; }

    int Throw();
}

class DeterministicDice : IDice
{
    private int _current = 1;

    public int Throws { get; private set; } = 0;

    public int Throw()
    {
        Throws++;
        return _current++;
    }
}
