using _23;
using Common;

var chars = Resources.GetResourceFile("input.txt")
    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Select(field => field).ToArray())
    .ToArray();

var world = new AmphipodWorld(chars);

Console.WriteLine(world);



static int Part1(AmphipodWorld world, IEnumerable<int> previousWorlds, int energyCost, int bestSolution = int.MaxValue)
{
    if (energyCost >= bestSolution)
    {
        return int.MaxValue;
    }

    if (world.IsFinished())
    {
        //Console.WriteLine($"Finished at {energyCost}");
        //Console.WriteLine();
        //Console.WriteLine(world);
        //Console.ReadLine();
        return energyCost;
    }

    foreach (var move in world.GetPossibleMoves())
    {
        var hash = move.World.GetHashCode();
        if (previousWorlds.Contains(hash))
        {
            continue;
        }

        if (energyCost + move.EnergyCost > bestSolution)
        {
            continue;
        }

        //Console.Clear();
        //Console.WriteLine(new string('>', previousWorlds.Count()));
        //Console.WriteLine(energyCost + move.EnergyCost);
        //Console.WriteLine(move.World);
        //Thread.Sleep(300);
        //Console.ReadLine();

        var newSolution = Part1(move.World, previousWorlds.Append(hash), energyCost + move.EnergyCost, bestSolution);

        if (newSolution < bestSolution)
        {
            bestSolution = newSolution;
            Console.Write($"\r{bestSolution}         {previousWorlds.Count()}          ");
        }
    }

    return bestSolution;
}

Console.WriteLine($"Part 1: {Part1(world, new[] { world.GetHashCode() }, 0)}");
