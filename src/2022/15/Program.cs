using System.Text.RegularExpressions;
using AdventOfCode.Common;

var inputRegex = new Regex(@"Sensor at x=(?<sX>-?\d+), y=(?<sY>-?\d+): closest beacon is at x=(?<bX>-?\d+), y=(?<bY>-?\d+)");

var sensors = Resources.GetInputFileLines()
    .Select(line => inputRegex.Match(line))
    .Select(match => (
        Sensor: new Coor(int.Parse(match.Groups["sY"].Value), int.Parse(match.Groups["sX"].Value)),
        Beacon: new Coor(int.Parse(match.Groups["bY"].Value), int.Parse(match.Groups["bX"].Value))))
    .Select(def => (
        def.Sensor,
        def.Beacon,
        Distance: Coor.ManhattanDistance(def.Sensor, def.Beacon)))
    .ToList();

static int GetImpossiblePositionCount(List<(Coor Sensor, Coor Beacon, int Distance)> sensors, int row)
{
    var minLeft = sensors.Min(s => s.Sensor.X - s.Distance);
    var maxLeft = sensors.Max(s => s.Sensor.X + s.Distance);
    return Enumerable.Range(minLeft, maxLeft - minLeft + 1)
        .Select(x => new Coor(row, x))
        .Where(coor => sensors.Any(s => s.Beacon != coor && Coor.ManhattanDistance(s.Sensor, coor) <= s.Distance))
        .Count();
}

Console.WriteLine(GetImpossiblePositionCount(sensors, 2000000));
