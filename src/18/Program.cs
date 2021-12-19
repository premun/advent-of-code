using _18;
using Common;

var parser = new SnailfishParser();
// var numbers = Resources.GetResourceFileLines("input.txt").Select(l => parser.Parse(l));

var number = parser.Parse("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]");

Console.WriteLine(number);
