using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace adventofcode23.Day1;

public class Day2
{
    private readonly ITestOutputHelper _output;

    public Day2(ITestOutputHelper output)
    {
        _output = output;
    }

    private record Game(int Id, List<Dictionary<string, int>> Sets);

    [Fact]
    public async Task Day2Part1()
    {
        var lines = await File.ReadAllLinesAsync("./Day2/input.txt");

        var games = ParseGames(lines);

        var gameCubes = new Dictionary<string, int>()
        {
            {"red", 12 },
            {"green", 13 },
            {"blue", 14 }
        };

        var possibleGames = games.Where(game =>
        {
            foreach (var set in game.Sets)
            {
                foreach (var subSet in set)
                {
                    if (!gameCubes.TryGetValue(subSet.Key, out var gameCubeVal))
                    {
                        return false;
                    }

                    if (subSet.Value > gameCubeVal)
                    {
                        return false;
                    }
                }
            }

            return true;
        });

        var result = possibleGames.Select(x => x.Id).Sum();

        _output.WriteLine($"Day 2p1:{result}");
    }

    [Fact]
    public async Task Day2Part2()
    {
        var lines = await File.ReadAllLinesAsync("./Day2/input.txt");

        var games = ParseGames(lines);

        var minReqPerGame = new Dictionary<int, Dictionary<string, int>>();

        foreach (var game in games)
        {
            var gameMinReq = new Dictionary<string, int>();

            foreach (var set in game.Sets)
            {
                foreach (var setColor in set)
                {
                    if (!gameMinReq.ContainsKey(setColor.Key) || gameMinReq[setColor.Key] < setColor.Value)
                    {
                        gameMinReq[setColor.Key] = setColor.Value;
                    }
                }
            }

            minReqPerGame[game.Id] = gameMinReq;
        }

        var gamePowers = minReqPerGame.Select(gameReq => new KeyValuePair<int, int>(gameReq.Key, gameReq.Value.Values.Aggregate((x, y) => x * y))).Select(x => x.Value).Sum();

        _output.WriteLine($"Day 2p2:{gamePowers}");
    }

    private IEnumerable<Game> ParseGames(string[] lines)
    {
        var games = lines.Select(line =>
        {
            var split = line.Split(":");
            var gameId = Regex.Replace(split[0], "Game ", "");
            var sets = split[1].Trim().Split(";").Select(set =>
            {
                var dict = new Dictionary<string, int>();
                // e.g. "3 blue, 4 red"
                set.Split(",").Select(x => x.Trim()).ToList().ForEach(x =>
                {
                    var splitCube = x.Split(" ");

                    dict.Add(splitCube[1], int.Parse(splitCube[0]));
                });

                return dict;
            });
            return new Game(int.Parse(gameId), sets.ToList());
        });

        return games;
    }
}
