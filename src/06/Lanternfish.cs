namespace _03;

internal class Lanternfish
{
    private int _timer;

    public Lanternfish(int timer)
    {
        _timer = timer;
    }

    public bool Tick()
    {
        if (_timer > 0)
        {
            _timer--;
            return false;
        }

        _timer = 6;
        return true;
    }
}
