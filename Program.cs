using System.Reflection;
using AdventOfCode._Shared;
using Microsoft.Extensions.DependencyInjection;

await Interface(BuildServiceProvider());

ServiceProvider BuildServiceProvider()
{
    var serviceCollection = new ServiceCollection();

    var days = Assembly.GetAssembly(typeof(Program))!
        .GetTypes()
        .Where(x => !x.IsAbstract && x.GetInterfaces().Contains(typeof(IDay)));

    foreach (var day in days)
    {
        ((ICollection<ServiceDescriptor>)serviceCollection).Add(
            new ServiceDescriptor(
                serviceType: typeof(IDay),
                implementationType: day,
                lifetime: ServiceLifetime.Singleton));
    }

    return serviceCollection.BuildServiceProvider();
}

async Task Interface(IServiceProvider services)
{
    Console.WriteLine("Advent of Code - Day Solver");
    var days = services.GetServices<IDay>().OrderBy(x => x.Day).ToList();

    if (days.IsNullOrEmpty()) return;

    Console.WriteLine("Choose day to solve:");
    Console.WriteLine();

    foreach (var day in days.OrderBy(x => x.Day))
    {
        Console.WriteLine($"[{day.Day}] - {day.DayName}");
    }

    Console.WriteLine("[Enter] - Latest");
    Console.WriteLine();

    var input = Console.ReadLine();

    if (!string.IsNullOrEmpty(input) && int.TryParse(input, out var inputDigit))
    {
        var day = days.FirstOrDefault(x => x.Day == inputDigit);

        if (day != null) await day.Solve();
        else await days.Last().Solve();
    }
    else await days.Last().Solve();
}