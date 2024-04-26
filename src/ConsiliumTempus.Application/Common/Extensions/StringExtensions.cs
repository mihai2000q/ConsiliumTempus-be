namespace ConsiliumTempus.Application.Common.Extensions;

public static class StringExtensions
{
    private static readonly char[] Numbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    private static readonly char[] Separators = [' ', '-'];

    public static string ToId(this string propertyName) => propertyName + "Id";

    public static string ToBackingField(this string propertyName) =>
        propertyName.Length switch
        {
            0 => "_",
            1 => $"_{propertyName.ToLower()}",
            _ => $"_{propertyName[0].ToString().ToLower()}{propertyName[1..]}"
        };

    public static string ToIdBackingField(this string propertyName) => ToBackingField(propertyName) + "Id";

    public static string TruncateAggregate(this string str) =>
        str.Replace("Aggregate", "");

    public static bool ContainsUppercase(this string str) =>
        str.Any(char.IsUpper);

    public static bool ContainsLowercase(this string str) =>
        str.Any(char.IsLower);

    public static bool ContainsNumber(this string str) =>
        str.Any(Numbers.Contains);

    public static string Capitalize(this string str)
    {
        if (str.Length == 0) return string.Empty;
        if (str.Length == 1) return str.ToUpper();
        foreach (var separator in Separators)
        {
            if (str.Contains(separator))
            {
                return str.CapitalizeEachWord(separator);
            }
        }

        return str.CapitalizeWord();
    }


    public static string CapitalizeWord(this string word) =>
        word.ToUpper()[0] + word.ToLower()[1..];

    private static string CapitalizeEachWord(this string str, char separator)
    {
        return string
            .Join(separator, str
                .Split(separator)
                .Select(CapitalizeWord));
    }
}