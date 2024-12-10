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
        MoveBlock(next.Id);
        next = forwardStream.GetNext();
    }

    while (last is FreeBlock)
    {
        last = backwardStream.GetNext();
    }

    MoveBlock(last.Id);

    next = forwardStream.GetNext();
    last = backwardStream.GetNext();
}

if (debug) Console.Write('.');

// We ended up somewhere in a single file block, we need to process it
if (last is FileBlock)
{
    var bit1 = forwardStream.Bit;
    var bit2 = backwardStream.Bit;

    while (bit1 <= bit2)
    {
        MoveBlock(last.Id);
        bit1++;
    }
}

Console.WriteLine();

Console.WriteLine($"Part 1: {checksum}");
Console.WriteLine($"Part 2: {""}");

void MoveBlock(int blockId)
{
    checksum += (ulong)(blockId * position);
    position++;
    if (debug) Console.Write((char)(blockId + '0'));
}

file class BlockStream
{
    protected readonly IEnumerator<int> _diskMap;
    protected int _id;
    protected int _bit;
    private bool _currentBlockIsFile = true;

    public BlockStream(IEnumerable<int> diskMap)
    {
        _diskMap = diskMap.GetEnumerator();
        _id = 0;
        _diskMap.MoveNext();
        _bit = _diskMap.Current;
    }

    protected virtual int Id => _id;
    public virtual int Bit => _diskMap.Current - _bit - 1;

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
}

file class ReverseBlockStream(int[] diskMap)
    : BlockStream(diskMap.Reverse())
{
    private readonly int _maxId = diskMap.Length / 2;
    protected override int Id => _maxId - _id;
    public override int Bit => _bit;
}

abstract file record Block(int Id, int Size);
file record FileBlock(int Id, int Size) : Block(Id, Size);
file record FreeBlock(int Id, int Size) : Block(Id, Size);
