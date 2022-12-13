using AdventOfCode._Shared;

namespace AdventOfCode.Day9;

public class Day9 : BaseDayWithPuzzleInput
{
    public override int Day => 9;
    public override string DayName => "Day 9: Rope Bridge";

    private const int RopeLengthChallenge1 = 2;
    private const int RopeLengthChallenge2 = 10;

    public override async Task SolveChallenge1()
    {
        var puzzleInput = await GetPuzzleInput();

        var rope = GenerateRope(RopeLengthChallenge1);

        var instructions = ParseInstructions(puzzleInput);

        var tailPositions = FollowInstructions(rope, instructions);

        PrintPositions(tailPositions);

        Console.WriteLine($"Challenge 1: The tail visited this many positions: {tailPositions.Count}");
    }

    public override async Task SolveChallenge2()
    {
        var puzzleInput = await GetPuzzleInput();

        var rope = GenerateRope(RopeLengthChallenge2);

        var instructions = ParseInstructions(puzzleInput);

        var tailPositions = FollowInstructions(rope, instructions);

        PrintPositions(tailPositions);

        Console.WriteLine($"Challenge 2: The tail visited this many positions: {tailPositions.Count}");
    }

    private (int x, int y)[] GenerateRope(int length)
    {
        var initialPosition = (x: 0, y: 0);

        var rope = new (int x, int y)[length];

        for (var i = 0; i < rope.Length; i++) rope[i] = initialPosition;

        return rope;
    }

    private ICollection<(int x, int y)> FollowInstructions((int x, int y)[] rope, ICollection<(Direction, int)> instructions)
    {
        var tailPositions = new HashSet<(int, int)> { rope[^1] };

        foreach (var (direction, steps) in instructions)
        {
            for (var i = 0; i < steps; i++)
            {
                var newRope = new (int x,int y)[rope.Length];
                newRope[0] = ProcessInstruction(direction, rope[0]);

                for (var j = 1; j < rope.Length; j++)
                {
                    newRope[j] = FollowParent(newRope[j - 1], rope[j], rope[j - 1]);
                }

                rope = newRope;

                tailPositions.Add(rope[^1]);
            }
        }

        return tailPositions;
    }

    private (int x, int y) ProcessInstruction(Direction direction, (int x, int y) head)
    {
        head = direction switch
        {
            Direction.U => (head.x, head.y + 1),
            Direction.D => (head.x, head.y - 1),
            Direction.L => (head.x - 1, head.y),
            Direction.R => (head.x + 1, head.y),
            _ => throw new ArgumentOutOfRangeException()
        };

        return head;
    }

    private (int x, int y) FollowParent((int x, int y) head, (int x, int y) tail, (int x, int y) historicHead)
    {
        if (IsAdjacent(head, tail)) return tail;

        var deltaX = head.x - tail.x;
        var deltaY = head.y - tail.y;

        // Y
        if (deltaX == 0 && deltaY != 0) tail.y += (deltaY + (deltaY < 0 ? 1 : -1));

        // X
        else if (deltaX != 0 && deltaY == 0) tail.x += (deltaX + (deltaX < 0 ? 1 : -1));

        // Diagonals
        else if (Math.Abs(deltaX) == 1 && deltaY != 0) tail = (head.x, tail.y + deltaY + (deltaY < 0 ? 1 : -1));

        // Diagonals
        else if (deltaX != 0 && Math.Abs(deltaY) == 1) tail = (tail.x + deltaX + (deltaX < 0 ? 1 : -1), head.y);

        // In case distance is longer (e.g. diagonally moving away) use the history to follow the parent
        else if (Math.Abs(deltaX) > 1 && Math.Abs(deltaY) > 1) tail = historicHead;

        return tail;
    }

    private void PrintPositions(ICollection<(int, int)> positions)
    {
        var xDim = positions.Max(pos => pos.Item1);
        var yDim = positions.Max(pos => pos.Item2);

        // The tail/head could go in negative coords
        var adjustX = Math.Abs(positions.Min(pos => pos.Item1));
        var adjustY = Math.Abs(positions.Min(pos => pos.Item2));

        var matrix = new string[yDim + adjustY + 1, xDim + adjustX + 1];

        positions.ForEach(pos =>
        {
            var (x, y) = pos;
            x += adjustX;
            y += adjustY;

            matrix[y, x] = "#";
        });

        for (var i = matrix.GetLength(0) - 1; i >= 0; i--)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                var s = matrix[i, j];
                Console.Write(string.IsNullOrEmpty(s) ? "." : s);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private bool IsAdjacent((int, int) t1, (int, int) t2)
    {
        var head = (x: t1.Item1, y: t1.Item2);
        var tail = (x: t2.Item1, y: t2.Item2);

        // overlap
        if (head == tail) return true;

        // X
        if (Math.Abs(head.x - tail.x) <= 1 && head.y == tail.y) return true;

        // y
        if (Math.Abs(head.y - tail.y) <= 1 && head.x == tail.x) return true;

        // diagonal
        if (Math.Abs(head.x - tail.x) == 1 && Math.Abs(head.y - tail.y) == 1) return true;

        return false;
    }

    private enum Direction
    {
        U,
        D,
        L,
        R
    }

    private (Direction, int)[] ParseInstructions(ICollection<string> input) =>
        input.Select(s =>
        {
            var split = s.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            Enum.TryParse(split[0], true, out Direction dir);

            return (dir, int.Parse(split[1]));
        }).ToArray();
}