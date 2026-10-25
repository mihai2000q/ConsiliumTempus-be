namespace ConsiliumTempus.Infrastructure.Extensions;

public static class StringExtensions
{
    extension(string str)
    {
        public string Underscore(string str2) => str + "_" + str2;

        public string FromPascalToCamelCase() => 
            str.Length switch
            {
                0 => string.Empty,
                1 => str[0].ToString().ToLower(),
                _ => str[0].ToString().ToLower() + str[1..]
            };

        public string ToId() => str + "Id";
        public string ToBackingField() => "_" + str.FromPascalToCamelCase();
        public string ToIdBackingField() => str.ToBackingField().ToId();
        public string TruncateAggregate() => str.Replace("Aggregate", "");
    }
}