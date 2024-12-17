using AdventOfCode.Common;

int[] diskMap = Resources.GetInputFileLines().First()
    .Select(c => c - '0')
    .ToArray();

var blocks = new LinkedList<Block>();
bool empty = false;

for (int i = 0; i < diskMap.Length; i++)
{
    blocks.AddLast(!empty
        ? new FileBlock((i + 1) / 2, i, diskMap[i])
        : new FreeBlock(i, diskMap[i]));
    empty = !empty;
}

LinkedListNode<Block> end = blocks.Last!;
while(end != blocks.First)
{
    LinkedListNode<Block> start = blocks.First!;
    while (start != end)
    {
        if (end.Value is not FileBlock fileToMove)
        {
            end = end.Previous!;
            continue;
        }

        if (start.Value is not FreeBlock free || free.Size < fileToMove.Size)
        {
            start = start.Next!;
            continue;
        }

        var newStart = start.Next;
        blocks.AddBefore(start, new FileBlock(fileToMove.FileId, fileToMove.BlockId, fileToMove.Size));
        free.Size -= fileToMove.Size;
        if (free.Size == 0)
        {
            blocks.Remove(start);
        }

        var newEmptySpace = blocks.AddBefore(end, new FreeBlock(0, fileToMove.Size));

        start = blocks.First!;
        end = end.Previous!;

        blocks.Remove(newEmptySpace.Next!);

        //Print();
    }

    end = end.Previous!;
}

ulong checksum = 0UL;
int id = 0;
foreach (var block in blocks)
{
    if (block is not FileBlock f)
    {
        id += block.Size;
        continue;
    }

    for (int j = 0; j < block.Size; j++)
    {
        checksum += (ulong)(id * f.FileId);
        id++;
    }
}

Console.WriteLine(checksum);

void Print()
{
    foreach (var block in blocks)
    {
        Console.Write(new string(block is FileBlock f ? (char)('0' + f.FileId) : '.', block.Size));
    }
    Console.WriteLine();
}

Console.WriteLine();

abstract file record Block(int BlockId, int Size)
{
    public int Size { get; set; } = Size;
}
file record FileBlock(int FileId, int BlockId, int Size) : Block(BlockId, Size);
file record FreeBlock(int BlockId, int Size) : Block(BlockId, Size);
