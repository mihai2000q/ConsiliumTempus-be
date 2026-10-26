namespace ConsiliumTempus.Common.IntegrationTests.Extensions;

// Copy of the (internal) extension from the Application, so the tests do not need access to its internals
public static class ListExtensions
{
    public static TReturn? IfNotEmpty<TSource, TReturn>(
        this IReadOnlyList<TSource> list,
        Func<IReadOnlyList<TSource>, TReturn> action)
    {
        return list.Count != 0 ? action.Invoke(list) : default;
    }
}
