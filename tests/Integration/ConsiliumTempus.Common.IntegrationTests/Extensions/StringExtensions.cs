namespace ConsiliumTempus.Common.IntegrationTests.Extensions;

// Copy of the (internal) extensions from the Application and the Infrastructure, so the tests do not need access to its internals
public static class StringExtensions
{
    private static readonly char[] Separators = [' ', '-'];

    extension(string str)
    {
        public string Capitalize()
        {
            var chars = str.ToLower().ToCharArray();
            var isStartOfWord = true;
            for (var i = 0; i < chars.Length; i++)
            {
                if (isStartOfWord) chars[i] = char.ToUpper(chars[i]);
                isStartOfWord = Separators.Contains(chars[i]);
            }

            return new string(chars);
        }

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
    }
}
