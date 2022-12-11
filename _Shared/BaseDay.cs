namespace AdventOfCode._Shared;

public abstract class BaseDay : IDay
{
    public abstract int Day { get; }
    public abstract string DayName { get; }

    public abstract Task SolveChallenge1();

    public abstract Task SolveChallenge2();
}