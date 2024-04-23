namespace ConsiliumTempus.Application.Common.Extensions;

public static class GlobalExtensions
{
    public static TReturn? IfNotNull<T, TReturn>(this T o, Func<TReturn> s) => o is null ? default : s.Invoke();
}