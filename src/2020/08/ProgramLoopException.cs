namespace AdventOfCode._2020_08;

class ProgramLoopException : Exception
{
    public int AccumulatorValue { get; }

    public ProgramLoopException(int accumulatorValue)
    {
        AccumulatorValue = accumulatorValue;
    }
}
