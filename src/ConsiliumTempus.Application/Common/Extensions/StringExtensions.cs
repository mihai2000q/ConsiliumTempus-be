namespace ConsiliumTempus.Application.Common.Extensions;

public static class StringExtensions
{
    private static readonly char[] Numbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

    public static string TruncateAggregate(this string str) =>
        str.Replace("Aggregate", "");

    public static bool ContainsUppercase(this string str) =>
        str.Any(char.IsUpper);

    public static bool ContainsLowercase(this string str) =>
        str.Any(char.IsLower);

    public static bool ContainsNumber(this string str) =>
        str.Any(Numbers.Contains);

    public static bool IsValidEmail(this string str) =>
        Regex.RegexStore.EmailRegex().IsMatch(str);

    public static string Capitalize(this string str)
    {
        var separators = new[] { " ", "-" };
        if (str.Length == 0) return string.Empty;
        if (str.Length == 1) return str.ToUpper();
        foreach (var separator in separators)
        {
            if (str.Contains(separator))
            {
                return str.CapitalizeEachWord(separator);
            }
        }

        return str.CapitalizeWord();
    }

    private static string CapitalizeEachWord(this string str, string separator)
    {
        return string
            .Join(separator, str
                .Split(separator)
                .Select(CapitalizeWord));
    }

    public static string CapitalizeWord(this string word) =>
        word.ToUpper()[0] + word.ToLower()[1..];
}