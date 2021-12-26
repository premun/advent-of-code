using _23;
using Common;

var world = new AmphipodWorld(Resources.GetResourceFile("input.txt"));

var finishedWorld = new AmphipodWorld(
    "#############" + Environment.NewLine +
    "#...........#" + Environment.NewLine +
    "###A#B#C#D###" + Environment.NewLine +
    "  #A#B#C#D#" + Environment.NewLine +
    "  #########");

static int Part1(AmphipodWorld startWorld, AmphipodWorld finishedWorld)
{
    // Dijkstra
    var queue = new PriorityQueue<AmphipodWorld, int>();
    queue.Enqueue(startWorld, 0);

    var lowestEnergies = new Dictionary<int, int>
    {
        [startWorld.GetHashCode()] = 0
    };

    var prev = new Dictionary<int, (AmphipodWorld, int)>();

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        var hash = current.GetHashCode();
        var currentEnergy = lowestEnergies[hash];

        foreach (var move in current.GetPossibleMoves())
        {
            var hash2 = move.World.GetHashCode();
            if (!lowestEnergies.TryGetValue(hash2, out var neighbourEnergy))
            {
                neighbourEnergy = int.MaxValue;
            }

            var newEnergy = currentEnergy + move.EnergyCost;
            if (newEnergy < neighbourEnergy)
            {
                lowestEnergies[hash2] = newEnergy;
                queue.Enqueue(move.World, newEnergy);
                prev[hash2] = (current, currentEnergy);
            }
        }
    }

    var start = startWorld.GetHashCode();
    var currentHash = finishedWorld.GetHashCode();
    var moves = new List<(AmphipodWorld, int)>();
    while (currentHash != start)
    {
        moves.Add(prev[currentHash]);
        currentHash = prev[currentHash].Item1.GetHashCode();
    }

    // Print the sequence
    int i = 1;
    foreach (var w in moves.Reverse<(AmphipodWorld, int)>())
    {
        Console.WriteLine($"{i++}# - {w.Item2}:");
        Console.WriteLine(w.Item1);
    }

    Console.WriteLine(lowestEnergies[finishedWorld.GetHashCode()]);
    Console.WriteLine(finishedWorld);

    return lowestEnergies[finishedWorld.GetHashCode()];
}

Console.WriteLine($"Part 1: {Part1(world, finishedWorld)}");
