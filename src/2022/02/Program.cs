using AdventOfCode.Common;

var rounds = Resources.GetResourceFileLines("input.txt").Select(l => (l[0], l[2])).ToList();

static int ChoiceScore(char choice) => choice < 'D'
    ? choice - 'A' + 1
    : choice - 'X' + 1;

var score1 = rounds.Sum(c =>
{
    (char c1, char c2) = c;
    var myScore = ChoiceScore(c2);

    if (ChoiceScore(c1) == myScore)
    {
        return myScore + 3;
    }

    return myScore + (c1, c2) switch
    {
        ('A', 'Y') => 6,
        ('A', 'Z') => 0,
        ('B', 'X') => 0,
        ('B', 'Z') => 6,
        ('C', 'X') => 6,
        /*('C', 'Y')*/_ => 0, 
    };
});

var score2 = rounds.Sum(c => c.Item2 switch
{
    'X' => 0 + c.Item1 switch
    {
        'A' => 3,
        'B' => 1,
        _ => 2
    },
    'Y' => 3 + ChoiceScore(c.Item1),
    _ => 6 + c.Item1 switch
    {
        'A' => 2,
        'B' => 3,
        _ => 1
    },
});

Console.WriteLine($"Part 1: {score1}");
Console.WriteLine($"Part 2: {score2}");
