namespace AdventOfCode._2021_09;

class Basin
{
    public int Size { get; set; }
}

class BasinMap
{
    private readonly int[][] _heatMap;
    private readonly Basin[,] _basinMap;
    private readonly List<Basin> _basins;

    public BasinMap(int[][] heatMap)
    {
        _heatMap = heatMap;
        _basinMap = new Basin[heatMap.Length, heatMap[0].Length];
        _basins = new();
    }

    public IEnumerable<Basin> FindBasins()
    {
        for (int row = 0; row < _heatMap.Length; row++)
        {
            for (int column = 0; column < _heatMap[row].Length; column++)
            {
                if (_basinMap[row, column] != null || _heatMap[row][column] == 9)
                {
                    continue;
                }

                var newBasin = new Basin();
                _basins.Add(newBasin);

                MarkBasin(newBasin, row, column);
            }
        }

        return _basins;
    }

    private void MarkBasin(Basin basin, int row, int column)
    {
        // Out of bounds
        if (row < 0 || column < 0 || row >= _heatMap.Length || column >= _heatMap[0].Length)
        {
            return;
        }

        // Already marked
        if (_basinMap[row, column] != null || _heatMap[row][column] == 9)
        {
            return;
        }

        basin.Size += 1;
        _basinMap[row, column] = basin;

        MarkBasin(basin, row + 1, column);
        MarkBasin(basin, row, column + 1);
        MarkBasin(basin, row - 1, column);
        MarkBasin(basin, row, column - 1);
    }
}
