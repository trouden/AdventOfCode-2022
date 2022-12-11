using System.Text;

namespace AdventOfCode._Shared;

public abstract class BaseDay : IDay
{
    public abstract int Day { get; }
    public abstract string DayName { get; }

    public abstract Task SolveChallenge1();

    public abstract Task SolveChallenge2();
}

public abstract class BaseDayWithPuzzleInput : BaseDay
{
    protected virtual string FolderName => $"Day{Day}";
    protected virtual string FileName => "PuzzleInput.txt";

    protected virtual async Task<ICollection<string>> GetPuzzleInput()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FolderName, FileName);
        return await GetInput(filePath);
    }

    protected virtual async Task<ICollection<string>> GetInput(string filePath)
        => await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
}