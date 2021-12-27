using _23;

static int FindLowestEnergyCost(AmphipodWorld startWorld, AmphipodWorld finishedWorld)
{
    Console.WriteLine("Searching from");
    Console.WriteLine(startWorld);

    // Dijkstra
    var queue = new PriorityQueue<AmphipodWorld, int>();
    queue.Enqueue(startWorld, 0);

    var lowestEnergies = new Dictionary<int, int>
    {
        [startWorld.GetHashCode()] = 0
    };

    var previousWorld = new Dictionary<int, (AmphipodWorld, int)>();

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        var currentEnergy = lowestEnergies[current.GetHashCode()];

        foreach (var move in current.GetPossibleMoves())
        {
            var hash = move.World.GetHashCode();
            if (!lowestEnergies.TryGetValue(hash, out var neighbourEnergy))
            {
                neighbourEnergy = int.MaxValue;
            }

            var newEnergy = currentEnergy + move.EnergyCost;
            if (newEnergy < neighbourEnergy)
            {
                lowestEnergies[hash] = newEnergy;
                queue.Enqueue(move.World, newEnergy);
                previousWorld[hash] = (current, currentEnergy);
            }
        }
    }

    var currentHash = finishedWorld.GetHashCode();
    if (!lowestEnergies.TryGetValue(currentHash, out var result))
    {
        Console.WriteLine("Failed to find the moves to reach the desired state!");
        return -1;
    }

    var start = startWorld.GetHashCode();
    var moves = new List<(AmphipodWorld, int)>();

    while (currentHash != start)
    {
        moves.Add(previousWorld[currentHash]);
        currentHash = previousWorld[currentHash].Item1.GetHashCode();
    }

    // Print the winning sequence
    int i = 1;
    foreach (var w in moves.Reverse<(AmphipodWorld, int)>())
    {
        Console.WriteLine($"{i++}# - {w.Item2}:");
        Console.WriteLine(w.Item1);
    }

    Console.WriteLine(result);
    Console.WriteLine(finishedWorld);

    return result;
}

var startWorld = new AmphipodWorld(new[]
{
    "#############",
    "#...........#",
    "###B#A#B#C###",
    "  #C#D#D#A#",
    "  #########"
});

var endWorld = new AmphipodWorld(new[]
{
    "#############",
    "#...........#",
    "###A#B#C#D###",
    "  #A#B#C#D#",
    "  #########"
});

Console.WriteLine($"Part 1: " + FindLowestEnergyCost(startWorld, endWorld));
Console.WriteLine();

startWorld = new AmphipodWorld(new[]
{
    "#############",
    "#...........#",
    "###B#A#B#C###",
    "  #D#C#B#A#",
    "  #D#B#A#C#",
    "  #C#D#D#A#",
    "  #########",
});

endWorld = new AmphipodWorld(new[]
{
    "#############",
    "#...........#",
    "###A#B#C#D###",
    "  #A#B#C#D#",
    "  #A#B#C#D#",
    "  #A#B#C#D#",
    "  #########"
});

Console.WriteLine($"Part 2: " + FindLowestEnergyCost(startWorld, endWorld));
