using AdventOfCode._Shared;

namespace AdventOfCode.Day4;

public class Day4 : BaseDayWithPuzzleInput
{
    public override int Day => 4;
    public override string DayName => "Day 4: Camp Cleanup";

    public override async Task SolveChallenge1()
    {
        var fullyContainedPairs = 0;

        var puzzleInput = await GetPuzzleInput();

        foreach (var pairAssignment in puzzleInput)
        {
            var (elf1, elf2) = GetPairAssignment(pairAssignment);

            if (ContainsOtherRange(elf1, elf2) || ContainsOtherRange(elf2, elf1))
                fullyContainedPairs++;
        }

        Console.WriteLine($"Challenge 1: there are this many fully contained pairs: {fullyContainedPairs}");
    }

    public override async Task SolveChallenge2()
    {
        var overlappingPairs = 0;

        var puzzleInput = await GetPuzzleInput();

        foreach (var pairAssignment in puzzleInput)
        {
            var (elf1, elf2) = GetPairAssignment(pairAssignment);

            if (ContainsOverlap(elf1, elf2) || ContainsOverlap(elf2, elf1))
                overlappingPairs++;
        }

        Console.WriteLine($"Challenge 2: there are this many pairs with overlap: {overlappingPairs}");
    }

    private static bool ContainsOtherRange((int, int) x, (int, int) y)
    {
        var item1Comparison = x.Item1.CompareTo(y.Item1);
        var item2Comparison = x.Item2.CompareTo(y.Item2);

        return item1Comparison <= 0 && item2Comparison >= 0;
    }

    private static bool ContainsOverlap((int, int) x, (int, int) y)
    {
        if (ContainsOtherRange(x, y)) return true;

        var item1Comparison1 = x.Item1.CompareTo(y.Item1);
        var item1Comparison2 = x.Item1.CompareTo(y.Item2);

        var item2Comparison1 = x.Item2.CompareTo(y.Item1);
        var item2Comparison2 = x.Item2.CompareTo(y.Item2);

        return (item1Comparison1 >= 0 && item1Comparison2 <= 0) || (item2Comparison1 >= 0 && item2Comparison2 <= 0);
    }

    private ((int, int), (int, int)) GetPairAssignment(string input)
    {
        var split = input.Split(',');

        if (split.Length != 2) throw new Exception($"Unexpected assignment: {input}.");

        return (GetElfAssignment(split[0]), GetElfAssignment(split[1]));
    }

    private (int, int) GetElfAssignment(string input)
    {
        var split = input.Split('-').Select(int.Parse).ToList();

        if (split.Count != 2) throw new Exception($"Unexpected assignment: {input}.");

        return (split[0], split[1]);
    }
}