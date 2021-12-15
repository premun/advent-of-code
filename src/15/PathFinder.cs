using Priority_Queue;

namespace _15;

class PathFinder
{
    private readonly int[][] _map;
    private readonly int _maxY;
    private readonly int _maxX;

    public PathFinder(int[][] map)
    {
        _map = map;
        _maxY = map.Length - 1;
        _maxX = map[0].Length - 1;
    }

    public int Search()
    {
        var shortestPaths = new int[_maxY + 1, _maxX + 1];
        var estimatedPaths = new int[_maxY + 1, _maxX + 1];

        for (int y = 0; y <= _maxY; y++)
        {
            for (int x = 0; x <= _maxX; x++)
            {
                shortestPaths[y, x] = int.MaxValue;
                estimatedPaths[y, x] = int.MaxValue;
            }
        }

        shortestPaths[0, 0] = 0;
        estimatedPaths[0, 0] = EstimateCost(new Coor(0, 0));

        var openSet = new SimplePriorityQueue<Coor, int>();
        openSet.Enqueue(new Coor(0, 0), estimatedPaths[0, 0]);

        while (openSet.Count > 0)
        {
            var current = openSet.First;

            if (current.X == _maxX && current.Y == _maxY)
            {
                return shortestPaths[_maxY, _maxX];
            }

            openSet.Dequeue();

            foreach (var neighbour in GetNeighbourhs(current))
            {
                var tentativeScore = shortestPaths[current.Y, current.X] + _map[current.Y][current.X];

                if (tentativeScore < shortestPaths[neighbour.Y, neighbour.X])
                {
                    shortestPaths[neighbour.Y, neighbour.X] = tentativeScore;
                    estimatedPaths[neighbour.Y, neighbour.X] = tentativeScore + EstimateCost(neighbour);

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Enqueue(neighbour, estimatedPaths[neighbour.Y, neighbour.X]);
                    }
                    else
                    {
                        openSet.UpdatePriority(neighbour, estimatedPaths[neighbour.Y, neighbour.X]);
                    }
                }
            }
        }

        throw new Exception("???");
    }

    private IEnumerable<Coor> GetNeighbourhs(Coor coor)
    {
        var neighbours = new[]
        {
            coor + new Coor(0, 1),
            coor + new Coor(1, 0),
            coor + new Coor(-1, 0),
            coor + new Coor(0, -1),
        };

        return neighbours.Where(n => !IsOutOfBounds(n));
    }

    private bool IsOutOfBounds(Coor coor) => coor.X < 0 || coor.Y < 0 || coor.Y > _maxY || coor.X > _maxX;

    private int EstimateCost(Coor coor) => _maxX - coor.X + _maxY - coor.Y;
}
