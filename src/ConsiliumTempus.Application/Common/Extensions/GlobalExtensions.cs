namespace ConsiliumTempus.Application.Common.Extensions;

public static class GlobalExtensions
{
    public static TS? IfNotNull<T, TS>(this T o, TS s) => o is null ? default : s;
}