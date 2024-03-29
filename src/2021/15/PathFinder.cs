﻿using Coor = AdventOfCode.Common.Coor<int>;

namespace AdventOfCode._2021_15;

class PathFinder(int[][] map)
{
    private readonly int[][] _map = map;
    private readonly int _maxY = map.Length - 1;
    private readonly int _maxX = map[0].Length - 1;

    public int FindShortestPath()
    {
        var shortestPaths = new int[_maxY + 1, _maxX + 1];

        for (int y = 0; y <= _maxY; y++)
        {
            for (int x = 0; x <= _maxX; x++)
            {
                shortestPaths[y, x] = int.MaxValue;
            }
        }

        shortestPaths[0, 0] = 0;

        var queue = new PriorityQueue<Coor, int>();
        queue.Enqueue(new Coor(0, 0), 0);

        while (queue.Count > 0)
        {
            var current = queue.Peek();

            if (current.X == _maxX && current.Y == _maxY)
            {
                // Corrections of the value for the sake of how we account for starting/ending fields
                // We don't count starting field but have to count last field
                return shortestPaths[_maxY, _maxX] - _map[0][0] + _map[_maxY][_maxY];
            }

            queue.Dequeue();

            foreach (var neighbour in GetNeighbourhs(current))
            {
                var tentativeScore = shortestPaths[current.Y, current.X] + _map[current.Y][current.X];
                if (tentativeScore < shortestPaths[neighbour.Y, neighbour.X])
                {
                    shortestPaths[neighbour.Y, neighbour.X] = tentativeScore;
                    queue.Enqueue(neighbour, tentativeScore);
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
}
