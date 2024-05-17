namespace ConsiliumTempus.Application.Common.Extensions;

public static class ListExtensions
{
    public static void IfNotEmpty<TSource>(this IReadOnlyList<TSource> list, Action<IReadOnlyList<TSource>> action)
    {
        if (list.Count != 0) action.Invoke(list);
    }
}