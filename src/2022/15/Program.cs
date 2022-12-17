using System.Text.RegularExpressions;
using AdventOfCode.Common;

var inputRegex = new Regex(@"Sensor at x=(?<sX>-?\d+), y=(?<sY>-?\d+): closest beacon is at x=(?<bX>-?\d+), y=(?<bY>-?\d+)");

var sensors = Resources.GetInputFileLines()
    .Select(line => inputRegex.Match(line))
    .Select(match => (
        Sensor: new Coor(int.Parse(match.Groups["sY"].Value), int.Parse(match.Groups["sX"].Value)),
        Beacon: new Coor(int.Parse(match.Groups["bY"].Value), int.Parse(match.Groups["bX"].Value))))
    .Select(def => new Sensor(def.Sensor, def.Sensor.ManhattanDistance(def.Beacon)))
    .ToList();

Console.WriteLine($"Part 1: {GetImpossiblePositionCount(sensors, 10)}");

var distressedBeacon = GetImpossiblePosition(sensors, new Coor(0, 0), new Coor(4000000, 4000000)).First();
Console.WriteLine($"Part 2: {4000000L * distressedBeacon.X + distressedBeacon.Y}");

static int GetImpossiblePositionCount(List<Sensor> sensors, int row)
{
    var minLeft = sensors.Min(s => s.Location.X - s.Radius);
    var maxLeft = sensors.Max(s => s.Location.X + s.Radius);
    return Enumerable.Range(minLeft, maxLeft - minLeft + 1)
        .Select(x => new Coor(row, x))
        .Where(coor => sensors.Any(s => coor.ManhattanDistance(s.Location) <= s.Radius))
        .Count();
}

static IEnumerable<Coor> GetImpossiblePosition(List<Sensor> sensors, Coor min, Coor max)
{
    for (int y = min.Y; y <= max.Y; ++y)
    for (int x = min.X; x <= max.X;)
    {
        var current = new Coor(y, x);
        var sensor = sensors.FirstOrDefault(s => current.ManhattanDistance(s.Location) <= s.Radius);
        if (sensor == null)
        {
            yield return new Coor(y, x);
            continue;
        }

        x = sensor.Location.X + sensor.Radius - Math.Abs(y - sensor.Location.Y) + 1;
    }
}

file record Sensor(Coor Location, int Radius);
