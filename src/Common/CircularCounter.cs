using System.Collections;

namespace AdventOfCode.Common;

public class CircularCounter(string instructions) : IEnumerable<char>
{
    public int Counter { get; set; }

    public IEnumerator<char> GetEnumerator() => new InstructionEnumerator(instructions, this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private class InstructionEnumerator(string instructions, CircularCounter parent) : IEnumerator<char>
    {
        private readonly string _instructions = instructions;
        private readonly CircularCounter _parent = parent;
        private int _index = 0;

        public char Current => _instructions[_index];

        object IEnumerator.Current => _instructions[_index];

        public void Dispose() { }

        public bool MoveNext()
        {
            _parent.Counter++;
            _index++;
            _index %= _instructions.Length;
            return true;
        }

        public void Reset()
        {
            _parent.Counter = 0;
            _index = 0;
        }
    }
}

