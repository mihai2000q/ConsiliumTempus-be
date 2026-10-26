namespace ConsiliumTempus.Common.IntegrationTests.Extensions;

// Copy of the (internal) extension from the Application, so the tests do not need access to its internals
public static class GlobalExtensions
{
    public static TReturn? IfNotNull<T, TReturn>(this T? o, Func<T, TReturn> s)
        => o is null ? default : s.Invoke((T)o);

    public static TReturn? IfNotNull<T, TReturn>(this T? o, Func<T, TReturn> s)
        where T : struct
        => o is null ? default : s.Invoke((T)o);
}
