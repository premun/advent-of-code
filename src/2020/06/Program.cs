using AdventOfCode.Common;

var groups = Resources.GetInputFileContent()
    .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.None)
    .Select(s => s.SplitBy(Environment.NewLine))
    .ToArray();

Console.WriteLine($"Part 1: {groups.Select(answers => string.Join(string.Empty, answers).Distinct().Count()).Sum()}");
Console.WriteLine($"Part 2: {groups.Select(groupAnswers => groupAnswers.SelectMany(personAnswers => personAnswers.Where(answer => groupAnswers.All(a => a.Contains(answer)))).Distinct().Count()).Sum()}");
