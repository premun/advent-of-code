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

var quantumGame = new QuantumDiracDiceGame(maxPosition: 10, winningPoints: 21, diceSides: 3);
var possibleWins1 = quantumGame.RunGame(startingPositions[0], startingPositions[1], true);
var possibleWins2 = quantumGame.RunGame(startingPositions[1], startingPositions[0], false);

Console.WriteLine($"Part 2: {Math.Max(possibleWins1, possibleWins2)}");

/*
var testData = new[]
{
    (1, 27, 0 ),
    (2, 183, 156 ),
    (3, 990, 207 ),
    (10, 18973591, 12657100 ),
    (21, 444356092776315, 341960390180808 ),
};

foreach (var data in testData)
{
    var quantumGame = new QuantumDiracDiceGame(maxPosition: 10, winningPoints: data.Item1, diceSides: 3);
    var possibleWins1 = quantumGame.RunGame(startingPositions[0], startingPositions[1], true) /27;
    var possibleWins2 = quantumGame.RunGame(startingPositions[1], startingPositions[0], false);

    var def = Console.ForegroundColor;
    Console.ForegroundColor = data.Item2 == possibleWins1 ? ConsoleColor.Green : ConsoleColor.Red;
    Console.Write($"{possibleWins1,17}");
    Console.ForegroundColor = def;
    Console.Write($" vs ");
    Console.ForegroundColor = data.Item3 == possibleWins2 ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine($"{possibleWins2,15}");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine($"{data.Item2,17}    {data.Item3,15}");
    Console.ForegroundColor = def;
}
*/
