using System.Text.RegularExpressions;
using AdventOfCode.Common;

var passports = Resources.GetResourceFile("input.txt")
    .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.None)
    .Select(passport => passport.Replace(Environment.NewLine, " "))
    .ToArray();

static bool HasAllProperties(string passport)
{
    var required = new[]
    {
        "byr",
        "iyr",
        "eyr",
        "hgt",
        "hcl",
        "ecl",
        "pid",
    };

    foreach (var prop in required)
    {
        if (!passport.Contains(prop + ":"))
        {
            return false;
        }
    }

    return true;
}

static bool IsValid(string passport)
{
    if (!HasAllProperties(passport))
    {
        return false;
    }

    var validators = new Dictionary<string, Func<string, bool>>
    {
        { "byr", byr => int.TryParse(byr, out var year) && year >= 1920 && year <= 2002 },
        { "iyr", iyr => int.TryParse(iyr, out var year) && year >= 2010 && year <= 2020 },
        { "eyr", eyr => int.TryParse(eyr, out var year) && year >= 2020 && year <= 2030 },
        { "hgt", hgt =>
            {
                var regex = new Regex("^(?<value>[0-9]+)(?<unit>in|cm)$");
                var match = regex.Match(hgt);

                if (!match.Success)
                {
                    return false;
                }

                if (match.Groups["unit"].Value == "cm")
                {
                    if (!int.TryParse(match.Groups["value"].ValueSpan, out var value))
                    {
                        return false;
                    }

                    return value >= 150 && value <= 193;
                }
                else if (match.Groups["unit"].Value == "in")
                {
                    if (!int.TryParse(match.Groups["value"].ValueSpan, out var value))
                    {
                        return false;
                    }

                    return value >= 59 && value <= 76;
                }
                else
                {
                    return false;
                }
            }
        },
        { "hcl", hcl => new Regex("^#[0-9a-f]{6}$").IsMatch(hcl) },
        { "ecl", ecl => new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(ecl) },
        { "pid", pid => new Regex("^[0-9]{9}$").IsMatch(pid) },
    };

    foreach (var part in passport.SplitBy(" "))
    {
        var parts = part.Split(":");
        var property = parts[0];
        var value = parts[1];

        if (validators.TryGetValue(property, out var validator) && !validator(value))
        {
            return false;
        }
    }

    return true;
}

Console.WriteLine($"Part 1: {passports.Count(HasAllProperties)}");
Console.WriteLine($"Part 2: {passports.Count(IsValid)}");
