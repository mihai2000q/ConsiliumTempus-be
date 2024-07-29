namespace ConsiliumTempus.Infrastructure.Extensions;

public static class StringExtensions
{
    public static string FromPascalToCamelCase(this string str) => 
        str.Length switch
        {
            0 => string.Empty,
            1 => str[0].ToString().ToLower(),
            _ => str[0].ToString().ToLower() + str[1..]
        };

    public static string ToId(this string propertyName) => propertyName + "Id";

    public static string ToBackingField(this string propertyName) => "_" + propertyName.FromPascalToCamelCase();

    public static string ToIdBackingField(this string propertyName) => ToBackingField(propertyName).ToId();

    public static string TruncateAggregate(this string str) => str.Replace("Aggregate", "");
}