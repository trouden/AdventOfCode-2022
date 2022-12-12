using System.Numerics;
using System.Runtime.InteropServices;
using AdventOfCode._Shared;

namespace AdventOfCode.Day8;

public class Day8 : BaseDayWithPuzzleInput
{
    public override int Day => 8;
    public override string DayName => "Day 8: Treetop Tree House";

    public override async Task SolveChallenge1()
    {
        var puzzleInput = (await GetPuzzleInput()).ToArray();

        var matrix = BuildMatrix(puzzleInput);

        var rows = matrix.GetLength(0);
        var columns = matrix.GetLength(1);

        var visibleTrees = (2 * columns) + (2 * rows) - 4; // -4 since corners are counted twice

        // Start at 1 to skip outer layer
        for (var i = 1; i < rows - 1; i++)
        {
            for (var j = 1; j < columns - 1; j++)
            {
                var tree = matrix[i, j];

                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    var treesInSight = GetTreesIndirection(matrix, i, j, direction);
                    if (treesInSight.Count > 0 && treesInSight.Max() < tree)
                    {
                        visibleTrees++;
                        break;
                    }
                }
            }
        }

        PrintMatrix(matrix);

        Console.WriteLine($"Challenge 1: This many trees are visible from the outside: {visibleTrees}");
    }

    public override async Task SolveChallenge2()
    {
        var puzzleInput = (await GetPuzzleInput()).ToArray();

        var matrix = BuildMatrix(puzzleInput);

        var rows = matrix.GetLength(0);
        var columns = matrix.GetLength(1);

        var scenicScore = 0;

        // Start at 1 to skip outer layer
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var tree = matrix[i, j];

                var score = 1;

                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    var treesInSight = GetTreesIndirection(matrix, i, j, direction);

                    var directionScore = treesInSight.Aggregate((score: 0, prevTree: 0), (aggregate, next) =>
                    {
                        if (aggregate.prevTree >= tree) return aggregate;

                        aggregate.score++;
                        aggregate.prevTree = next;

                        return aggregate;
                    });

                    score *= directionScore.score;
                }

                if (score > scenicScore) scenicScore = score;
            }
        }

        PrintMatrix(matrix);

        Console.WriteLine($"Challenge 2: This highest scenic score is: {scenicScore}.");
    }

    private int[,] BuildMatrix(string[] input)
    {
        var columns = input[0].Length;
        var rows = input.Length;

        var matrix = new int[rows, columns];

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                matrix[i, j] = int.Parse(input[i][j].ToString());
            }
        }

        return matrix;
    }

    enum Direction
    {
        North,
        East,
        South,
        West
    }

    private ICollection<int> GetTreesIndirection(int[,] matrix, int startRow, int startColumn, Direction direction = Direction.North)
    {
        var trees = new List<int>();

        if (Direction.North == direction)
        {
            for (var i = startRow - 1; i >= 0; i--)
            {
                trees.Add(matrix[i, startColumn]);
            }
        }
        else if (Direction.East == direction)
        {
            for (var i = startColumn + 1; i < matrix.GetLength(1); i++)
            {
                trees.Add(matrix[startRow, i]);
            }
        }
        else if (Direction.South == direction)
        {
            for (var i = startRow + 1; i < matrix.GetLength(0); i++)
            {
                trees.Add(matrix[i, startColumn]);
            }
        }
        else if (Direction.West == direction)
        {
            for (var i = startColumn - 1; i >= 0; i--)
            {
                trees.Add(matrix[startRow, i]);
            }
        }

        return trees.ToArray();
    }

    private void PrintMatrix(int[,] matrix)
    {
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i, j]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}