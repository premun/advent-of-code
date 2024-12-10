using AdventOfCode.Common;

int[] diskMap = Resources.GetInputFileLines().First()
    .Select(c => c - '0')
    .ToArray();

var debug = true;

var forward = new BlockStream(diskMap);
var backward = new BlockStream(diskMap.Reverse());
var blockCount = diskMap.Length;
var fileBlockCount = blockCount / 2;
var position = 0;

ulong checksum = 0UL;

do
{
    if (forward.Current is FileBlock first)
    {
        StoreBlock(first.FileId);
        forward.MoveNext();
        continue;
    }

    if (backward.Current is not FileBlock last)
    {
        backward.MoveNext();
        continue;
    }

    StoreBlock(fileBlockCount - last.FileId);
    forward.MoveNext();
    backward.MoveNext();
} while (forward.Current.BlockId < blockCount - backward.Current.BlockId - 1);

if (debug) Console.Write('.');

while (backward.Current is FileBlock f && f.Bit < diskMap[forward.Current.BlockId] - forward.Current.Bit)
{
    StoreBlock(fileBlockCount - f.FileId);
    backward.MoveNext();
}

Console.WriteLine();
Console.WriteLine($"Part 1: {checksum}");
Console.WriteLine($"Part 2: {""}");

void StoreBlock(int fileId)
{
    checksum += (ulong)(fileId * position);
    position++;
    if (debug) Console.Write((char)(fileId + '0'));
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
        _bit = 0;
        _diskMap.MoveNext();
    }

    public Block Current => _currentBlockIsFile
        ? new FileBlock(_fileId, _blockId, _bit)
        : new FreeBlock(_blockId, _bit);

    public bool MoveNext()
    {
        _bit++;

        // Moving onto a new file block
        while (_bit == _diskMap.Current)
        {
            if (!_diskMap.MoveNext())
            {
                return false;
            }

            _bit = 0;
            _currentBlockIsFile = !_currentBlockIsFile;
            if (_currentBlockIsFile)
            {
                _fileId++;
            }
            _blockId++;
        }

        return true;
    }
}

abstract file record Block(int BlockId, int Bit);
file record FileBlock(int FileId, int BlockId, int Bit) : Block(BlockId, Bit);
file record FreeBlock(int BlockId, int Bit) : Block(BlockId, Bit);
