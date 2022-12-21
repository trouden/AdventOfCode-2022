using AdventOfCode._Shared;

namespace AdventOfCode.Day10;

public class Day10 : BaseDayWithPuzzleInput
{
    public override int Day => 10;
    public override string DayName => "Day 10: Cathode-Ray Tube";

    public override async Task SolveChallenge1()
    {
        var puzzleInput = (await GetPuzzleInput()).ToArray();

        var register = 1;
        var registerList = new List<int> { register };

        var cycle = 0;

        var cycleTracker = 20;

        var tracker = new List<int>();

        var addStack = new Queue<int?>();

        var instructionIndex = 0;

        while (instructionIndex < puzzleInput.Length || addStack.Count > 0)
        {
            cycle++;

            if (cycle == cycleTracker)
            {
                Console.WriteLine($"Values: {string.Join(", ", registerList)}");
                Console.WriteLine($"Value: {registerList.Sum()}");
                Console.WriteLine();

                cycleTracker += 40;
                tracker.Add(register * cycle);
            }

            Console.WriteLine($"Cycle: {cycle}: {register} [{string.Join(", ", addStack.Select(x => x?.ToString() ?? "null").ToList())}]");

            // Add value if needed
            if (addStack.Count > 0)
            {
                var v = addStack.Dequeue() ?? 0;
                register += v;

                if (v != 0)
                {
                    registerList.Add(v);
                }

                continue;
            }

            var input = instructionIndex < puzzleInput.Length ? puzzleInput[instructionIndex] : null;

            if (input is null) continue;

            instructionIndex++;

            var split = input.Split();

            if (split.Length == 1) continue;

            var add = split[1];

            addStack.Enqueue(int.Parse(add));
        }

        Console.WriteLine($"Cycle values: {string.Join(", ", tracker)}");

        Console.WriteLine($"Challenge 1: Sum of the signal strengths is: {tracker.Sum()}.");
    }

    public override async Task SolveChallenge2()
    {
        var puzzleInput = (await GetPuzzleInput()).ToArray();

        var register = 1;
        var registerList = new List<int> { register };

        var cycle = 0;

        var cycleTracker = 20;

        var tracker = new List<int>();

        var addStack = new Queue<int?>();

        var instructionIndex = 0;

        var crt = new string[6, 40];

        while (instructionIndex < puzzleInput.Length || addStack.Count > 0)
        {
            cycle++;

            {
                var y = (cycle - 1) % 40;
                var x = Convert.ToInt32(Math.Round((double)((cycle - 1) / 40)));

                crt[x, y] = register == y || register - 1 == y || register + 1 == y ? "#" : ".";
            }

            if (cycle == cycleTracker)
            {
                Console.WriteLine($"Values: {string.Join(", ", registerList)}");
                Console.WriteLine($"Value: {registerList.Sum()}");
                Console.WriteLine();

                cycleTracker += 40;
                tracker.Add(register * cycle);
            }

            Console.WriteLine($"Cycle: {cycle}: {register} [{string.Join(", ", addStack.Select(x => x?.ToString() ?? "null").ToList())}]");

            // Add value if needed
            if (addStack.Count > 0)
            {
                var v = addStack.Dequeue() ?? 0;
                register += v;

                if (v != 0)
                {
                    registerList.Add(v);
                }

                continue;
            }

            var input = instructionIndex < puzzleInput.Length ? puzzleInput[instructionIndex] : null;

            if (input is null) continue;

            instructionIndex++;

            var split = input.Split();

            if (split.Length == 1) continue;

            var add = split[1];

            addStack.Enqueue(int.Parse(add));
        }

        Console.WriteLine($"Challenge 2: ");
        PrintCrt(crt);
    }

    private void PrintCrt(string[,] crt)
    {
        for (var i = 0; i < crt.GetLength(0); i++)
        {
            for (var j = 0; j < crt.GetLength(1); j++)
            {
                Console.Write(crt[i, j]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}