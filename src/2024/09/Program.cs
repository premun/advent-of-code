using AdventOfCode.Common;

int[] diskMap = Resources.GetInputFileLines().First()
    .Select(c => c - '0')
    .ToArray();

var forwardStream = new BlockStream(diskMap);
var backwardStream = new ReverseBlockStream(diskMap);

ulong checksum = 0UL;
var next = forwardStream.GetNext()!;
var last = backwardStream.GetNext()!;
int position = 0;

var debug = true;

while (next.Id < last.Id)
{
    while (next is FileBlock)
    {
        checksum += (ulong)(next.Id * position);
        position++;
        if (debug) Console.Write((char)(next.Id + '0'));
        next = forwardStream.GetNext();
    }

    while (last is FreeBlock)
    {
        last = backwardStream.GetNext();
    }

    checksum += (ulong)(last.Id * position);
    position++;
    if (debug) Console.Write((char)(last.Id + '0'));

    next = forwardStream.GetNext();
    last = backwardStream.GetNext();
}

if (debug) Console.Write('.');

// We ended up somewhere in a single file block, we need to process it
if (next is FileBlock)
{
    // TODO: This could be better - we could make the pointers meet inside the block
    var sum = 0UL;
    for (int i = 0; i < diskMap.Length; i += 2)
        sum += (ulong)diskMap[i];

    for (ulong i = (ulong)position; i < sum; ++i)
    {
        checksum += (ulong)(last.Id * position);
        position++;
        if (debug) Console.Write((char)(last.Id + '0'));
    }
}

Console.WriteLine();

Console.WriteLine($"Part 1: {checksum}");
Console.WriteLine($"Part 2: {""}");

file class BlockStream
{
    private readonly IEnumerator<int> _diskMap;
    protected int _id;
    private int _bit;
    private bool _currentBlockIsFile = true;

    public BlockStream(IEnumerable<int> diskMap)
    {
        _diskMap = diskMap.GetEnumerator();
        _id = 0;
        _diskMap.MoveNext();
        _bit = _diskMap.Current;
    }

    public Block GetNext()
    {
        while (_bit == 0)
        {
            if (!_diskMap.MoveNext())
            {
                return new FreeBlock(Id, int.MaxValue);
            }

            _currentBlockIsFile = !_currentBlockIsFile;
            _bit = _diskMap.Current;

            if (_currentBlockIsFile)
            {
                _id++;
            }
        }

        _bit--;
        return _currentBlockIsFile
            ? new FileBlock(Id, _diskMap.Current)
            : new FreeBlock(Id, _diskMap.Current);
    }

    protected virtual int Id => _id;
}

file class ReverseBlockStream(int[] diskMap)
    : BlockStream(diskMap.Reverse())
{
    private readonly int _maxId = diskMap.Length / 2;

    protected override int Id => _maxId - _id;
}

abstract file record Block(int Id, int Size);
file record FileBlock(int Id, int Size) : Block(Id, Size);
file record FreeBlock(int Id, int Size) : Block(Id, Size);
