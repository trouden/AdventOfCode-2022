namespace AdventOfCode._Shared;

public static class IEnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach(var item in enumerable) action(item);
    }

    public static IEnumerable<T[]> Window<T>(this IEnumerable<T> enumerable, int windowSize)
    {
        var array = enumerable.ToArray();

        if (windowSize > array.Length)
            throw new ArgumentException(windowSize.ToString(), nameof(windowSize), null);

        for (var i = 0; i + windowSize <= array.Length; i++)
        {
            yield return array[i..(i + windowSize)];
        }
    }
}