namespace ConsiliumTempus.Domain.Common.Extensions;

internal static class EnumerableExtensions
{
    internal static void ForEach<TSource>(this IEnumerable<TSource> enumerable, Action<TSource> action)
    {
        foreach (var source in enumerable)
        {
            action(source);
        }
    }
}