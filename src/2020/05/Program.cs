using AdventOfCode.Common;

var seats = Resources.GetInputFileContent()
    .Replace("F", "0")
    .Replace("B", "1")
    .Replace("R", "1")
    .Replace("L", "0")
    .SplitBy(Environment.NewLine)
    .Select(number => (Row: Convert.ToInt32(number.Substring(0, 7), 2), Column: Convert.ToInt32(number.Substring(7), 2)))
    .Select(s => (Row: s.Row, Column: s.Column, Id: s.Row * 8 + s.Column))
    .OrderBy(s => s.Id)
    .ToList();

Console.WriteLine($"Part 1: {seats.Last().Id}");

var allIds = Enumerable.Range(seats.First().Id, seats.Last().Id - seats.First().Id);
var missingIds = allIds.Except(seats.Select(s => s.Id));

Console.WriteLine($"Part 2: {missingIds.First(id => seats.Any(s => s.Id == id - 1) && seats.Any(s => s.Id == id + 1))}");
