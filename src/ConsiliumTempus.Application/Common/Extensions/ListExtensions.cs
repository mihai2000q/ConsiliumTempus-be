namespace ConsiliumTempus.Application.Common.Extensions;

public static class ListExtensions
{
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