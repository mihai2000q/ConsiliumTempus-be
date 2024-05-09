namespace ConsiliumTempus.Application.Common.Extensions;

public static class GlobalExtensions
{
    public static TReturn? IfNotNull<T, TReturn>(this T? o, Func<T, TReturn> s)
        => o is null ? default : s.Invoke((T)o);

    public static TReturn? IfNotNull<T, TReturn>(this T? o, Func<T, TReturn> s)
        where T : struct
        => o is null ? default : s.Invoke((T)o);
}