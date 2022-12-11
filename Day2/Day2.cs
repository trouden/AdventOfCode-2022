using AdventOfCode._Shared;

namespace AdventOfCode.Day2;

public class Day2 : BaseDayWithPuzzleInput
{
    public override int Day => 2;
    public override string DayName => "Day 2: Rock Paper Scissors";

    private enum RockPaperScissors
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    private class RockPaperScissorsComparer : IComparer<RockPaperScissors>
    {
        public RockPaperScissors LosesTo(RockPaperScissors input) =>
            input switch
            {
                RockPaperScissors.Rock => RockPaperScissors.Paper,
                RockPaperScissors.Paper => RockPaperScissors.Scissors,
                RockPaperScissors.Scissors => RockPaperScissors.Rock,

                _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
            };

        public RockPaperScissors WinsFrom(RockPaperScissors input) =>
            input switch
            {
                RockPaperScissors.Rock => RockPaperScissors.Scissors,
                RockPaperScissors.Paper => RockPaperScissors.Rock,
                RockPaperScissors.Scissors => RockPaperScissors.Paper,

                _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
            };

        public int Compare(RockPaperScissors x, RockPaperScissors y)
        {
            if (x == y) return 3;

            return WinsFrom(x) == y ? 6 : 0;
        }
    }

    public override async Task SolveChallenge1()
    {
        var input = await GetPuzzleInput();

        var parsedInput = input
            .Select(i => i.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(
                i =>
                {
                    var parsed = i.Select(s => s switch
                    {
                        "A" or "X" => RockPaperScissors.Rock,
                        "B" or "Y" => RockPaperScissors.Paper,
                        "C" or "Z" => RockPaperScissors.Scissors,
                        _ => throw new ArgumentOutOfRangeException(nameof(s), s, null)
                    }).ToList();

                    return (opponent: parsed[0], me: parsed[1]);
                })
            .ToList();

        var points = 0;

        var comparer = new RockPaperScissorsComparer();

        foreach (var round in parsedInput)
        {
            points += (int)round.me;
            points += comparer.Compare(round.me, round.opponent);
        }

        Console.WriteLine($"Challenge 1: Expected amount of points: {points}");
    }

    public override async Task SolveChallenge2()
    {
        var input = await GetPuzzleInput();

        var comparer = new RockPaperScissorsComparer();

        var parsedInput = input
            .Select(i => i.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(
                i =>
                {
                    var opponent = i[0] switch
                    {
                        "A" => RockPaperScissors.Rock,
                        "B" => RockPaperScissors.Paper,
                        "C" => RockPaperScissors.Scissors,
                        _ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
                    };

                    // X -> lose
                    // Y -> draw
                    // Z -> win
                    var me = i[1] switch
                    {
                        "X" => comparer.WinsFrom(opponent),
                        "Y" => opponent,
                        "Z" => comparer.LosesTo(opponent),
                        _ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
                    };

                    return (opponent: opponent, me: me);
                })
            .ToList();

        var points = 0;

        foreach (var round in parsedInput)
        {
            points += (int)round.me;
            points += comparer.Compare(round.me, round.opponent);
        }

        Console.WriteLine($"Challenge 2: Expected amount of points: {points}");
    }
}