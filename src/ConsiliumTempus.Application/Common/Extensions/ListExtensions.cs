namespace ConsiliumTempus.Application.Common.Extensions;

public static class ListExtensions
{
    public static bool IsEmpty<TSource>(this IReadOnlyList<TSource> list) => list.Count == 0;

    public static bool IsNotEmpty<TSource>(this IReadOnlyList<TSource> list) => list.Count > 0;

    public static void IfNotEmpty<TSource>(this IReadOnlyList<TSource> list, Action<IReadOnlyList<TSource>> action)
    {
        if (list.Count != 0) action.Invoke(list);
    }

    public static TReturn? IfNotEmpty<TSource, TReturn>(
        this IReadOnlyList<TSource> list,
        Func<IReadOnlyList<TSource>, TReturn> action)
    {
        return list.Count != 0 ? action.Invoke(list) : default;
    }
}