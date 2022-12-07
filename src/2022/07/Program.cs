using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

static Directory ParseFileTree(string[] input)
{
    var root = new Directory("/", null, new());
    Directory currentDir = root;

    var commandLine = new Queue<string>(input);
    while (commandLine.TryDequeue(out string? line))
    {
        if (line.StartsWith("$ cd "))
        {
            currentDir = line.Substring(5) switch
            {
                "/" => root,
                ".." => currentDir.Parent!,
                string name => (Directory)(currentDir.Items.FirstOrDefault(i => i is Directory && i.Name == name)
                    ?? new Directory(name, currentDir, new()))
            };

            continue;
        }

        if (line == "$ ls")
        {
            while (commandLine.TryPeek(out var child) && !child.StartsWith('$'))
            {
                child = commandLine.Dequeue();
                if (child is ['d', 'i', 'r', ' ', .. string name])
                {
                    currentDir.Items.Add(new Directory(name, currentDir, new()));
                }
                else
                {
                    var parts = child.Split(' ', 2);
                    currentDir.Items.Add(new File(parts[1], currentDir, long.Parse(parts[0])));
                }
            }
        }
    }

    return root;
}

static Dictionary<string, long> GetDirectorySizes(Directory root, string path = "/", Dictionary<string, long>? sizes = null)
{
    sizes ??= new Dictionary<string, long>();

    long size = root.Items
        .Select(item =>
        {
            if (item is File file) return file.Size;

            var directory = (Directory)item;
            var itemPath = path + "/" + item.Name;
            GetDirectorySizes(directory, itemPath, sizes);
            return sizes[itemPath];
        })
        .Sum();

    sizes.Add(path, size);
    return sizes;
}

Directory root = ParseFileTree(lines);
var orderedSizes = GetDirectorySizes(root).Select(kv => kv.Value).Order().ToList();

const long diskSize = 70000000L;
const long requiredFreeSpace = 30000000L;
var freeSpace = diskSize - orderedSizes.Last();
var needsToDelete = requiredFreeSpace - freeSpace;

Console.WriteLine($"Part 1: {orderedSizes.TakeWhile(size => size <= 100000).Sum()}");
Console.WriteLine($"Part 2: {orderedSizes.First(size => size >= needsToDelete)}");

abstract record Item(string Name, Directory? Parent);
record Directory(string Name, Directory? Parent, HashSet<Item> Items) : Item(Name, Parent);
record File(string Name, Directory? Parent, long Size) : Item(Name, Parent);
