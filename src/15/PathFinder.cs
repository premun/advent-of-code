namespace _15;

class PathFinder
{
    private readonly int[][] _map;
    private readonly int[,] _shortestMap;
    private readonly int _maxY;
    private readonly int _maxX;

    public PathFinder(int[][] map)
    {
        _map = map;
        _maxY = map.Length;
        _maxX = map[0].Length;

        _shortestMap = new int[_maxY, _maxX];
    }

    public int CurrentShortestPath => _shortestMap[_maxY - 1, _maxX - 1];

    public int Search()
    {
        for (int y = 0; y < _maxY; y++)
        {
            for (int x = 0; x < _maxX; x++)
            {
                _shortestMap[y, x] = int.MaxValue;
            }
        }

        Search(0, new() { new Coor(0, 0) });
        return CurrentShortestPath;
    }

    private void Search(int currentLength, List<Coor> currentPath)
    {
        if (currentLength >= CurrentShortestPath)
        {
            return;
        }

        var position = currentPath.Last();

        if (_shortestMap[position.Y, position.X] < currentLength)
        {
            return;
        }

        _shortestMap[position.Y, position.X] = currentLength;

        if (position == new Coor(_maxY - 1, _maxX - 1))
        {
            Console.WriteLine(CurrentShortestPath);
            return;
        }

        var neighbours = GetNeighbourhs(position)
            .Where(n => _shortestMap[n.Y, n.X] >= currentLength)
            .Where(n => !currentPath.Contains(n));

        foreach (var neighbour in neighbours)
        {
            Search(currentLength + _map[position.Y][position.X], currentPath.Append(neighbour).ToList());
        }
    }

    private Coor[] GetNeighbourhs(Coor coor)
    {
        var neighbours = new[]
        {
            coor + new Coor(0, 1),
            coor + new Coor(1, 0),
            coor + new Coor(-1, 0),
            coor + new Coor(0, -1),
        };

        return neighbours.Where(n => !IsOutOfBounds(n)).ToArray();
    }

    private bool IsOutOfBounds(Coor coor) => coor.X < 0 || coor.Y < 0 || coor.Y >= _maxY || coor.X >= _maxX;
}
