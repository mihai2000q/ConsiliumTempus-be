namespace ConsiliumTempus.Application.Common.Extensions;

public static class StringExtensions
{
    private static readonly char[] Numbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

    public static bool ContainsUppercase(this string str) =>
        str.Any(char.IsUpper);

    public static bool ContainsLowercase(this string str) =>
        str.Any(char.IsLower);

    public static bool ContainsNumber(this string str) =>
        str.Any(Numbers.Contains);

    public static bool IsValidEmail(this string str) =>
        Regex.RegexStore.EmailRegex().IsMatch(str);
}