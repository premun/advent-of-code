using System.Collections;
using System.Text;
using AdventOfCode.Common;

Dictionary<string, string[]> modulesAndDestinations = Resources
    .GetInputFileLines()
    .Select(s => s.Replace(" ", null))
    .Select(s => s.Split("->", 2))
    .ToDictionary(p => p[0], p => p[1].SplitBy(","));

Dictionary<string, Module> modules = modulesAndDestinations.Keys
    .Select(Module.Parse)
    .ToDictionary(m => m.Name, m => m);

List<Output> outputs = modulesAndDestinations.Values
    .SelectMany(p => p)
    .Distinct()
    .Where(name => !modules.ContainsKey(name))
    .Select(name => new Output(name))
    .ToList();

foreach (var output in outputs)
{
    modules.Add(output.Name, output);
}

foreach (var pair in modulesAndDestinations)
{
    var module = modules[pair.Key.Replace("%", null).Replace("&", null)];
    foreach (var dest in pair.Value)
    {
        module.Destinations.Add(modules[dest]);
    }
}

//var stateSize = modules.Values.Aggregate(0, (acc, module) => module switch
//{
//    Conjuction c => acc + c.Destinations.Count,
//    FlipFlop => acc + 1,
//    _ => acc,
//});

(var lows, var highs) = Simulate(modules.Values, 1000);

Console.WriteLine($"{lows}, {highs} = {lows * highs}");

static (long Lows, long Highs) Simulate(IReadOnlyCollection<Module> modules, int numOfPushes)
{
    var broadcaster = modules.OfType<Broadcaster>().First();
    var button = new Button(broadcaster);

    long lows = 0;
    long highs = 0;
    for (int i = 1; i <= numOfPushes; i++)
    {
        (var l, var h) = SendPulse(new Pulse(button, false));
        lows += l;
        highs += h;

        Console.WriteLine($"{(l, h)} -> ({lows}, {highs})");

        if (IsReset(modules))
        {
            Console.WriteLine($"Found a reset after {i} rounds");
            var pushesLeft = numOfPushes - i;
            lows *= pushesLeft / i;
            highs *= pushesLeft / i;
            i = numOfPushes - (pushesLeft % i);
        }

        Console.WriteLine();
    }

    Console.WriteLine($"{lows}, {highs} = {lows * highs}");

    return (lows, highs);
}

static (int Low, int High) SendPulse(Pulse firstPulse)
{
    int low = 0;
    int high = 0;

    var pulseQueue = new Queue<Pulse>([firstPulse]);
    while (pulseQueue.TryDequeue(out var item))
    {
        var module = item.Module;
        var pulse = item.IsHigh;
        var newPulse = module.SendPuls(pulse);

        foreach (var nextModule in module.Destinations)
        {
            if (newPulse)
            {
                high++;
            }
            else
            {
                low++;
            }

            Console.WriteLine($"{module.Name} -{(newPulse ? "high" : "low")}-> {nextModule.Name}");
            if (nextModule.ReceivePulse(module, newPulse))
            {
                pulseQueue.Enqueue(new Pulse(nextModule, newPulse));
            }
        }
    }

    return (low, high);
}

static bool IsReset(IReadOnlyCollection<Module> modules)
    => !modules.Any(m => (m is FlipFlop f && f.IsOn) || (m is Conjuction c && !c.AllDown));

//static BitArray GetState(int size, IReadOnlyCollection<Module> modules)
//{
//    var state = new BitArray(size + 1);
//    var i = -1;
//    foreach (var module in modules)
//    {
//        if (module is FlipFlop flipFlop)
//        {
//            i++;
//            if (flipFlop.IsOn)
//            {
//                state[i] = true;
//            }
//        }

//        if (module is Conjuction conjuction)
//        {
//            foreach (var m in conjuction.Memory.Values)
//            {
//                i++;
//                if (m)
//                {
//                    state[i] = true;
//                }
//            }
//        }
//    }

//    var sb = new StringBuilder();

//    for (int j = 0; j < state.Count; j++)
//    {
//        char c = state[j] ? '1' : '0';
//        sb.Append(c);
//    }

//    Console.WriteLine(sb.ToString());

//    return state;
//}

abstract file class Module(string name)
{
    public string Name { get; } = name;

    public List<Module> Destinations { get; } = [];

    public abstract bool SendPuls(bool isHigh);

    public abstract bool ReceivePulse(Module from, bool isHigh);

    public static Module Parse(string name) => name switch
    {
        ['%', .. string n] => new FlipFlop(n),
        ['&', .. string n] => new Conjuction(n),
        "broadcaster" => new Broadcaster(),
        _ => throw new Exception(name),
    };
}

file class FlipFlop(string name) : Module(name)
{
    public bool IsOn { get; private set; } = false;

    public override bool ReceivePulse(Module from, bool isHigh)
    {
        if (isHigh) return false;

        IsOn = !IsOn;
        return true;
    }

    public override bool SendPuls(bool isHigh) => IsOn;
}

file class Conjuction(string name) : Module(name)
{
    private int _bitsUp = 0;

    public bool AllUp => _bitsUp == Destinations.Count;

    public bool AllDown => _bitsUp == 0;

    public Dictionary<Module, bool> Memory { get; } = [];

    public override bool ReceivePulse(Module from, bool isHigh)
    {
        if (!Memory.TryGetValue(from, out bool wasHigh))
        {
            wasHigh = false;
        }

        if (wasHigh != isHigh)
        {
            _bitsUp += isHigh ? 1 : -1;
            Memory[from] = isHigh;
        }

        return true;
    }

    public override bool SendPuls(bool isHigh) => !AllUp;
}

file class Broadcaster() : Module("broadcaster")
{
    public override bool ReceivePulse(Module from, bool isHigh) => true;
    public override bool SendPuls(bool isHigh) => isHigh;
}

file class Output(string name) : Module(name)
{
    public override bool ReceivePulse(Module from, bool isHigh) => false;
    public override bool SendPuls(bool isHigh) => throw new NotSupportedException();
}

file class Button : Module
{
    public Button(Broadcaster broadcaster) : base("button")
    {
        Destinations.Add(broadcaster);
    }

    public override bool ReceivePulse(Module from, bool isHigh) => true;
    public override bool SendPuls(bool isHigh) => false;
}

file record Pulse(Module Module, bool IsHigh);
