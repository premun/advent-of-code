using _21;
using Common;

var startingPositions = Resources.GetResourceFileLines("input.txt")
    .Select(line => line.Split(" ").Last())
    .Select(int.Parse)
    .ToArray();

var dice = new DeterministicDice();
var players = new SimpleDiracDiceGame(dice, maxPosition: 10, winningPoints: 1000)
    .RunGame(startingPositions);

Console.WriteLine($"Part 1: {dice.Throws * players.Select(p => p.Points).Min()}");

foreach (var points in new[] { 1, 2, 3, 21 })
{
    var quantumGame = new QuantumDiracDiceGame(maxPosition: 10, winningPoints: points, diceSides: 3);
    var possibleWins1 = quantumGame.RunGame(startingPositions[0], startingPositions[1], true);
    var possibleWins2 = quantumGame.RunGame(startingPositions[1], startingPositions[0], false);

    Console.WriteLine($"Part 2:   {possibleWins1,15} vs {possibleWins2,15}");
}

Console.WriteLine($"Expected: {27,15} vs {0,15}");
Console.WriteLine($"Expected: {183,15} vs {156,15}");
Console.WriteLine($"Expected: {990,15} vs {207,15}");
Console.WriteLine("Expected: 444356092776315 vs 341960390180808");

// 11666337147
// 444356092776315
