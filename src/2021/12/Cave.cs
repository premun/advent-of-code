namespace AdventOfCode._2021_12;

class Cave
{
    public string Name { get; set; }

    public bool IsSmall => Name[0] >= 'a' && Name[0] <= 'z';

    public List<Cave> Paths = [];

    public Cave(string name) => Name = name;
}
