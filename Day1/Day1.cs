using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace adventofcode23.Day1;

public class Day1
{
    private readonly ITestOutputHelper _output;

    public Day1(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Day1Part1()
    {
        var inp = await File.ReadAllTextAsync("Day1/input.txt");

        var resultPart1 = inp.Trim()
            .Split("\r\n")
            .Select(x => Regex.Replace(x, "[A-Za-z]", ""))
            .Select(x => int.Parse(x.First().ToString() + x.Last().ToString()))
            .Sum();

        Assert.Equal(55607, resultPart1);
    }

    [Fact]
    public async Task Day1Part2()
    {
        var inp = await File.ReadAllTextAsync("Day1/input.txt");

        var numbers = new Dictionary<string, string>()
        {
            { "one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" }
        };

        var lines = inp.Trim().Split("\r\n");

        var newLines = new List<string>();

        foreach (var x in lines)
        {
            var numbaz = new List<string>();

            for (var idx = 0; idx < x.Length + 1; idx++)
            {
                for (var endIdx = idx + 1; endIdx < x.Length + 1; endIdx++)
                {
                    var currStr = x[idx..endIdx];


                    if (endIdx - idx == 1 && int.TryParse(currStr, out _))
                    {
                        numbaz.Add(currStr);
                        idx++;
                        continue;
                    }

                    if (numbers.ContainsKey(currStr))
                    {
                        numbaz.Add(numbers[currStr].ToString());
                        endIdx = idx + 1;
                        idx++;
                        continue;
                    }
                }
            }

            newLines.Add(string.Join("", numbaz));
        }

        var result = newLines
            .Select(x => int.Parse(x.First().ToString() + x.Last().ToString()))
            .Sum();

        Assert.Equal(55291, result);
    }
}
