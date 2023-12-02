using System.Text.RegularExpressions;
using AdventOfCode.Common;

// Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
var inputRegex = new Regex("Game (?<gameId>\\d+):");
var revealRegex = new Regex("(?<count>\\d+) (?<color>blue|red|green)");
var games = new List<Game>();

foreach (var line in Resources.GetInputFileLines())
{
    var id = int.Parse(inputRegex.Match(line).Groups["gameId"].Value);
    var reveals = line
        .SplitBy(";")
        .Select(reveals => revealRegex.Matches(reveals))
        .Select(reveals => reveals.Select(reveal =>
            (Count: int.Parse(reveal.Groups["count"].Value),
             Color: reveal.Groups["color"].Value)))
        .Select(reveals => new Reveal(
            Red: reveals.FirstOrDefault(r => r.Color == "red").Count,
            Blue: reveals.FirstOrDefault(r => r.Color == "blue").Count,
            Green: reveals.FirstOrDefault(r => r.Color == "green").Count))
        .ToList();

    games.Add(new Game(id, reveals));
}

var part1Bag = new Reveal(Red: 12, Green: 13, Blue: 14);
var possibleGames = games
    .Where(game => game.Reveals.All(reveal =>
        reveal.Red <= part1Bag.Red &&
        reveal.Green <= part1Bag.Green &&
        reveal.Blue <= part1Bag.Blue));

var leastCubes = games
    .Select(game => new Reveal(
        Red: game.Reveals.Max(reveal => reveal.Red),
        Green: game.Reveals.Max(reveal => reveal.Green),
        Blue: game.Reveals.Max(reveal => reveal.Blue)))
    .Select(leastCubes => leastCubes.Red * leastCubes.Blue * leastCubes.Green);

Console.WriteLine($"Part 1: {possibleGames.Select(game => game.Id).Sum()}");
Console.WriteLine($"Part 2: {leastCubes.Sum()}");

file record Reveal(int Blue, int Red, int Green);
file record Game(int Id, IReadOnlyCollection<Reveal> Reveals);
