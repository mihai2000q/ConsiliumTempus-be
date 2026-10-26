namespace ConsiliumTempus.Application.Common.Extensions;

internal static class EnumerableExtensions
{
    public static void ForEach<TSource>(this IEnumerable<TSource> enumerable, Action<TSource> action)
    {
        foreach (var source in enumerable) action(source);
    }
}