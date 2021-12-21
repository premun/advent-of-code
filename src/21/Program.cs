using _21;
using Common;

var startingPositions = Resources.GetResourceFileLines("input.txt")
    .Select(line => line.Split(" ").Last())
    .Select(int.Parse)
    .ToArray();

var dice = new DeterministicDice();
var game = new DiracDiceGame(dice, startingPositions, 10, 1000);

var players = game.RunGame();

Console.WriteLine($"Part 1: {dice.Throws * players.Select(p => p.Points).Min()}");
