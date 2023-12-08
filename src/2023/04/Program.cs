using AdventOfCode.Common;

IReadOnlyCollection<Card> cards = Resources.GetInputFileLines()
    .Select(Card.Parse)
    .ToArray();

var scores = cards
    .Where(card => card.MatchingNumbers > 0)
    .Select(card => Math.Pow(2, card.MatchingNumbers - 1));

var wonCards = cards.ToDictionary(c => c.Id, c => 1L);
for (int i = 0; i < cards.Count; i++)
{
    var card = cards.ElementAt(i);
    for (int j = 1; j <= card.MatchingNumbers && j + card.Id <= cards.Count; j++)
    {
        wonCards[card.Id + j] += wonCards[card.Id];
    }
}

Console.WriteLine($"Part 1: {scores.Sum()}");
Console.WriteLine($"Part 2: {wonCards.Values.Sum()}");

file record Card(int Id, int MatchingNumbers)
{
    public static Card Parse(string definition)
    {
        var digits = definition.ParseNumbersOut();

        const int numOfNumbers = 10;
        var winningNumbers = digits.Skip(1).Take(numOfNumbers);
        var numbers = digits.Skip(numOfNumbers + 1);

        return new Card(digits.First(), winningNumbers.Intersect(numbers).Count());
    }
}
