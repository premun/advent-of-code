namespace AdventOfCode._2020_08;

class ProgramLoopException(int accumulatorValue) : Exception
{
    public int AccumulatorValue { get; } = accumulatorValue;
}
