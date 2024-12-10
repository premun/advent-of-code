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

while (forwardStream < backwardStream)
{
    while (next is FileBlock)
    {
        MoveBlock(next.Id);
        next = forwardStream.GetNext();

        if (forwardStream > backwardStream)
            break;
    }

    while (last is FreeBlock)
    {
        last = backwardStream.GetNext();

        if (forwardStream > backwardStream)
            break;
    }

    MoveBlock(last.Id);

    next = forwardStream.GetNext();
    if (forwardStream > backwardStream)
        break;
    last = backwardStream.GetNext();

    if (forwardStream > backwardStream)
        break;
}

if (debug) Console.Write('.');

// We ended up somewhere in a single file block, we need to process it
//if (last is FileBlock)
//{
//    while (forwardStream != backwardStream)
//    {
//        MoveBlock(last.Id);
//        last = backwardStream.GetNext();
//    }
//}

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
    protected int _blockId;
    protected int _fileId;
    protected int _bit;
    private bool _currentBlockIsFile = true;

    public BlockStream(IEnumerable<int> diskMap)
    {
        _diskMap = diskMap.GetEnumerator();
        _blockId = 0;
        _diskMap.MoveNext();
        _bit = 0;
    }

    protected virtual int Bit => _bit - 1;
    protected virtual int BlockId => _blockId;
    protected virtual int FileId => _fileId;

    public Block GetNext()
    {
        _bit++;
        while (_bit == _diskMap.Current)
        {
            if (!_diskMap.MoveNext())
            {
                return new FreeBlock(FileId, int.MaxValue);
            }

            _bit = 0;
            _currentBlockIsFile = !_currentBlockIsFile;
            if (_currentBlockIsFile)
            {
                _fileId++;
            }
            _blockId++;
        }

        return _currentBlockIsFile
            ? new FileBlock(FileId, _diskMap.Current)
            : new FreeBlock(_blockId, _diskMap.Current);
    }

    public static bool operator <(BlockStream first, BlockStream second)
    {
        if (first.BlockId < second.BlockId) return true;
        if (first.BlockId == second.BlockId) return first.Bit < second.Bit;
        return false;
    }

    public static bool operator >(BlockStream first, BlockStream second)
    {
        return second < first;
    }

    public static bool operator ==(BlockStream first, BlockStream second)
    {
        return first.BlockId == second.BlockId && first.Bit == second.Bit;
    }

    public static bool operator !=(BlockStream first, BlockStream second)
    {
        return first.BlockId != second.BlockId || first.Bit == second.Bit;
    }
}

file class ReverseBlockStream(int[] diskMap)
    : BlockStream(diskMap.Reverse())
{
    private readonly int _maxFileId = diskMap.Length / 2;
    private readonly int _maxBlockId = diskMap.Length - 1;

    protected override int FileId => _maxFileId - _fileId;
    protected override int Bit => _diskMap.Current - _bit - 1;
    protected override int BlockId => _maxBlockId - _blockId;
}

abstract file record Block(int Id, int Size);
file record FileBlock(int Id, int Size) : Block(Id, Size);
file record FreeBlock(int Id, int Size) : Block(Id, Size);
