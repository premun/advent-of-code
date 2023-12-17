namespace AdventOfCode._2021_12;

class Cave(string name)
{
    public string Name { get; set; } = name;

    public bool IsSmall => Name[0] >= 'a' && Name[0] <= 'z';

    public List<Cave> Paths = [];
}
