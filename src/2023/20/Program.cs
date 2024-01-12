using AdventOfCode.Common;

Dictionary<string, string[]> modulesAndDestinations = Resources
    .GetInputFileLines()
    .Select(s => s.Replace(" ", null))
    .Select(s => s.Split("->", 2))
    .ToDictionary(p => p[0], p => p[1].SplitBy(","));

Dictionary<string, Module> modules = modulesAndDestinations.Keys
    .Select(Module.Parse)
    .ToDictionary(m => m.Name, m => m);

// These only appear on the right side
List<Output> outputs = modulesAndDestinations.Values
    .SelectMany(p => p)
    .Distinct()
    .Except(modules.Keys)
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
        var destination = modules[dest];
        module.Destinations.Add(destination);
        if (destination is Conjuction c)
        {
            c.Memory[module] = false;
        }
    }
}

modules["button"] = new Button(modules.Values.OfType<Broadcaster>().First());

(var lows, var highs) = Simulate(modules, 1000);

Console.WriteLine($"{lows}, {highs} = {lows * highs}");

static (int Lows, int Highs) Simulate(Dictionary<string, Module> modules, int numOfPushes)
{
    var button = modules.Values.OfType<Button>().Single();

    int lows = 0;
    int highs = 0;
    for (int i = 1; i <= numOfPushes; i++)
    {
        try
        {
            (var l, var h) = SendPulse(new Pulse(button, false));
            lows += l;
            highs += h;
        }
        catch (RxHitWithLowPulseException)
        {
            return (i, -1);
        }
    }

    return (lows, highs);
}

static (int, int) SendPulse(Pulse firstPulse)
{
    int lows = 0;
    int highs = 0;

    var pulseQueue = new Queue<Pulse>([firstPulse]);
    while (pulseQueue.TryDequeue(out var item))
    {
        var module = item.Module;
        var pulse = item.IsHigh;

        if (pulse)
        {
            highs += module.Destinations.Count;
        }
        else
        {
            lows += module.Destinations.Count;
        }

        foreach (var nextModule in module.Destinations)
        {
            if (nextModule.Name == "rx" && pulse == false)
            {
                throw new RxHitWithLowPulseException();
            }

            //Console.WriteLine($"{module.Name} -{(pulse ? "high" : "low")}-> {nextModule.Name}");
            var nextPulse = nextModule.ReceivePulse(module, pulse);
            if (nextPulse.HasValue)
            {
                pulseQueue.Enqueue(new Pulse(nextModule, nextPulse.Value));
            }
        }
    }

    return (lows, highs);
}

static bool IsReset(IReadOnlyCollection<Module> modules)
    => !modules.Any(m => (m is FlipFlop f && f.IsOn) || (m is Conjuction c && !c.AllDown));

abstract file class Module(string name)
{
    public string Name { get; } = name;

    public List<Module> Destinations { get; } = [];

    public abstract bool? ReceivePulse(Module from, bool isHigh);

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

    public override bool? ReceivePulse(Module from, bool isHigh)
    {
        if (isHigh) return null;

        IsOn = !IsOn;
        return IsOn;
    }
}

file class Conjuction(string name) : Module(name)
{
    private int _bitsUp = 0;

    public bool AllUp => _bitsUp == Memory.Count;

    public bool AllDown => _bitsUp == 0;

    public Dictionary<Module, bool> Memory { get; } = [];

    public override bool? ReceivePulse(Module from, bool isHigh)
    {
        var wasHigh = Memory[from];

        if (wasHigh != isHigh)
        {
            _bitsUp += isHigh ? 1 : -1;
            Memory[from] = isHigh;
        }

        return !AllUp;
    }
}

file class Broadcaster() : Module("broadcaster")
{
    public override bool? ReceivePulse(Module from, bool isHigh) => isHigh;
}

file class Output(string name) : Module(name)
{
    public override bool? ReceivePulse(Module from, bool isHigh) => null;
}

file class Button : Module
{
    public Button(Broadcaster broadcaster) : base("button")
    {
        Destinations.Add(broadcaster);
    }

    public override bool? ReceivePulse(Module from, bool isHigh) => false;
}

file record Pulse(Module Module, bool IsHigh);

file class RxHitWithLowPulseException() : Exception();
