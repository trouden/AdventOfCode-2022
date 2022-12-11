using AdventOfCode._Shared;

namespace AdventOfCode.Day3;

public class Day3 : BaseDayWithPuzzleInput
{
    public override int Day => 3;
    public override string DayName => "Day 3: Rucksack Reorganization";

    public override async Task SolveChallenge1()
    {
        var puzzleInput = await GetPuzzleInput();

        var sumOfPriorities = 0;

        foreach (var rucksack in puzzleInput)
        {
            var half = rucksack.Length / 2;

            var compartment1 = rucksack[..half];
            var compartment2 = rucksack[half..];

            sumOfPriorities += compartment1.Where(c => compartment2.Contains(c)).Distinct().Select(CharToPriority).Sum();
        }

        Console.WriteLine($"Challenge 1: Sum of priorities is: {sumOfPriorities}");
    }

    public override async Task SolveChallenge2()
    {
        var puzzleInput = await GetPuzzleInput();

        var sumOfPriorities = 0;

        foreach (var rucksacks in puzzleInput.Chunk(3))
        {
            sumOfPriorities += rucksacks[0]
                .Where(c =>
                {
                    for (var i = 1; i < rucksacks.Length; i++)
                    {
                        if (!rucksacks[i].Contains(c)) return false;
                    }

                    return true;
                })
                .Distinct()
                .Select(CharToPriority)
                .Sum();
        }

        Console.WriteLine($"Challenge 2: Sum of grouped priorities is: {sumOfPriorities}");
    }

    private static int CharToPriority(char c)
    {
        var intValue = (int)c;

        if (char.IsUpper(c))
        {
            intValue -= 65;
            intValue += 26;
        }
        else intValue -= 97;

        // go from 0-based to 1-based
        intValue += 1;

        return intValue;
    }
}