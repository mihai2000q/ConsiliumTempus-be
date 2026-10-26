namespace ConsiliumTempus.Common.IntegrationTests.Extensions;

// Copy of the (internal) extensions from the Application and the Infrastructure, so the tests do not need access to its internals
public static class StringExtensions
{
    private static readonly char[] Separators = [' ', '-'];

    extension(string str)
    {
        public string Capitalize()
        {
            if (str.Length == 0) return string.Empty;
            if (str.Length == 1) return str.ToUpper();
            foreach (var separator in Separators)
            {
                if (str.Contains(separator))
                {
                    return string.Join(separator, str.Split(separator).Select(w => w.CapitalizeWord()));
                }
            }

            return str.CapitalizeWord();
        }

        private string CapitalizeWord() =>
            str.ToUpper()[0] + str.ToLower()[1..];

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
