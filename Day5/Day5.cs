using System.Reflection.Emit;
using AdventOfCode._Shared;

namespace AdventOfCode.Day5;

public class Day5 : BaseDayWithPuzzleInput
{
    public override int Day => 5;
    public override string DayName => "Day 5: Supply Stacks";

    private const char PlaceHolderChar = '|';
    private const int StackInfoLineNumber = 9;

    public override async Task SolveChallenge1()
    {
        var puzzleInput = (await GetPuzzleInput()).ToArray();

        var stackInfo = puzzleInput[..StackInfoLineNumber];
        var instructions = puzzleInput[(StackInfoLineNumber+1)..];

        var listOfStacks = ParseStacks(stackInfo);

        PrintStacks(listOfStacks);

        foreach (var instruction in instructions.Select(ParseInstruction))
        {
            ExecuteInstructionWithQueue(listOfStacks, instruction);
        }

        PrintStacks(listOfStacks);

        Console.WriteLine($"Challenge 1: The top container of each layer is: {string.Join("", GetTopContainers(listOfStacks))}");
    }

    public override async Task SolveChallenge2()
    {
        var puzzleInput = (await GetPuzzleInput()).ToArray();

        var stackInfo = puzzleInput[..StackInfoLineNumber];
        var instructions = puzzleInput[(StackInfoLineNumber + 1)..];

        var listOfStacks = ParseStacks(stackInfo);

        PrintStacks(listOfStacks);

        foreach (var instruction in instructions.Select(ParseInstruction))
        {
            ExecuteInstructionWithStack(listOfStacks, instruction);
        }

        PrintStacks(listOfStacks);

        Console.WriteLine($"Challenge 2: The top container of each layer is: {string.Join("", GetTopContainers(listOfStacks))}");
    }

    private char[] GetTopContainers(Stack<char>[] listOfStacks)
    {
        var containers = new char[listOfStacks.Length];

        for (var i = 0; i < listOfStacks.Length; i++)
        {
            var stack = listOfStacks[i];
            if (stack.Count < 0) continue;

            containers[i] = stack.ToArray()[0];
        }

        return containers;
    }

    private char[] GetLayer(Stack<char>[] listOfStacks, int layer)
    {
        var containers = new char[listOfStacks.Length];

        for (var i = 0; i < listOfStacks.Length; i++)
        {
            var stack = listOfStacks[i];
            if (stack.Count < layer + 1) continue;

            // Layer is the inverse of stack index (e.g. bottom is actually the last element in the set)
            containers[i] = stack.ToArray()[^(layer + 1)]; 
        }

        return containers;
    }

    private record Instruction
    {
        public int Amount { get; init; }
        public int From { get; init; }
        public int To { get; init; }

        public override string ToString()
        {
            return $"move {Amount} from {From} to {To}";
        }
    }

    private Instruction ParseInstruction(string input)
    {
        var split = input
            .Split(' ')
            .Select(s => int.TryParse(s, out var i) ? i : (int?)null)
            .Where(i => i.HasValue)
            .Select(i => i!.Value)
            .ToArray();

        if (split.Length != 3)
            throw new ArgumentException(input, nameof(input));

        return new Instruction
        {
            Amount = split[0],
            From = split[1],
            To = split[2]
        };
    }

    private void ExecuteInstructionWithQueue(Stack<char>[] listOfStacks, Instruction instruction)
    {
        var tempQueue = new Queue<char>();

        for (var i = 0; i < instruction.Amount; i++)
            tempQueue.Enqueue(listOfStacks[instruction.From - 1].Pop());

        foreach (var container in tempQueue)
            listOfStacks[instruction.To - 1].Push(container);
    }

    private void ExecuteInstructionWithStack(Stack<char>[] listOfStacks, Instruction instruction)
    {
        var tempStack = new Stack<char>();

        for (var i = 0; i < instruction.Amount; i++)
            tempStack.Push(listOfStacks[instruction.From - 1].Pop());

        foreach (var container in tempStack)
            listOfStacks[instruction.To - 1].Push(container);
    }

    private Stack<char>[] ParseStacks(string[] stackInfo)
    {
        var amountOfStacks = stackInfo[^1]
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Max();

        var listOfStacks = new Stack<char>[amountOfStacks];
        for (var i = 0; i < amountOfStacks; i++) listOfStacks[i] = new Stack<char>();

        foreach (var info in stackInfo[..^1].Reverse())
        {
            var containers = info
                .Replace("    ", " " + PlaceHolderChar + " ")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Length > 1 ? s.Where(char.IsAsciiLetter).First() : s[0])
                .Select(s => s!)
                .ToList();

            for (var i = 0; i < containers.Count; i++)
            {
                var container = containers[i];
                if (container == PlaceHolderChar) continue;

                listOfStacks[i].Push(container);
            }
        }

        return listOfStacks;
    }

    private void PrintStacks(ICollection<Stack<char>> listOfStacks)
    {
        // Writeline will print the highest lines first so we invert the stack
        for (var i = listOfStacks.Select(x => x.Count).Max() - 1; i >= 0; i--)
        {
            foreach (var stack in listOfStacks)
            {
                if (i < stack.Count)
                {
                    // But to get the correct element we need to invert again.
                    Console.Write($"[{stack.ToArray()[^(i+1)]}]");
                }
                else
                {
                    Console.Write("   ");
                }

                Console.Write(' ');
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }
}