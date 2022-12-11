namespace AdventOfCode._Shared;

public interface IDay
{
    int Day { get; }
    string DayName { get; }
    Task SolveChallenge1();
    Task SolveChallenge2();

    async Task Solve()
    {
        await SolveChallenge1();
        await SolveChallenge2();
    }
}