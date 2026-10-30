namespace ConsiliumTempus.Domain.Common.Extensions;

internal static class StringExtensions
{
    internal static string UpperCamelCaseToSnakeCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        var result = str[0].ToString().ToLower();
        for (var i = 1; i < str.Length; i++)
        {
            result += char.IsUpper(str[i]) ? "_" + str[i].ToString().ToLower() : str[i];
        }

        return result;
    }
}
