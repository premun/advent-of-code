using _18;
using Common;

var parser = new SnailfishParser();
// var numbers = Resources.GetResourceFileLines("input.txt").Select(l => parser.Parse(l));

var number = parser.Parse("[[[[[9,8],1],2],3],4]");

Console.WriteLine();
