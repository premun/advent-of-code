using AdventOfCode._2021_21;
using AdventOfCode.Common;

var startingPositions = Resources.GetInputFileLines()
    .Select(line => line.Split(" ").Last())
    .Select(int.Parse)
    .ToArray();

var dice = new DeterministicDice();
var players = new SimpleDiracDiceGame(dice, maxPosition: 10, winningPoints: 1000)
    .RunGame(startingPositions);

Console.WriteLine($"Part 1: {dice.Throws * players.Select(p => p.Points).Min()}");

var quantumGame = new QuantumDiracDiceGame(maxPosition: 10, winningPoints: 21, diceSides: 3);
var possibleWins1 = quantumGame.RunGame(startingPositions[0], startingPositions[1], true);
var possibleWins2 = quantumGame.RunGame(startingPositions[1], startingPositions[0], false);

Console.WriteLine($"Part 2: {Math.Max(possibleWins1, possibleWins2)}");
