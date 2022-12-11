using AdventOfCode._Shared;

namespace AdventOfCode.Day6;

public class Day6 : BaseDayWithPuzzleInput
{
    public override int Day => 6;
    public override string DayName => "Day 6: Tuning Trouble";

    public override async Task SolveChallenge1()
    {
        var puzzleInput = (await GetPuzzleInput()).Single();

        const int windowSize = 4;

        var i = windowSize;

        var found = false;

        foreach (var window in puzzleInput.ToCharArray().Window(windowSize))
        {
            if (window.Distinct().Count() == windowSize)
            {
                found = true;
                break;
            }
            i++;
        }

        Console.WriteLine(found
            ? $"Challenge 1: the start sequence is after this many characters: {i}"
            : "Challenge 1: no start sequence found.");
    }

    public override async Task SolveChallenge2()
    {
        var puzzleInput = (await GetPuzzleInput()).Single();

        const int windowSize = 14;

        var i = windowSize;

        var found = false;

        foreach (var window in puzzleInput.ToCharArray().Window(windowSize))
        {
            if (window.Distinct().Count() == windowSize)
            {
                found = true;
                break;
            }
            i++;
        }

        Console.WriteLine(found
            ? $"Challenge 2: the message sequence is after this many characters: {i}"
            : "Challenge 2: no message sequence found.");
    }
}