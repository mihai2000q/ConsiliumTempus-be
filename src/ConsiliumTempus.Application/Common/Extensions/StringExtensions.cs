namespace ConsiliumTempus.Application.Common.Extensions;

public static class StringExtensions
{
    private static readonly char[] Numbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    private static readonly char[] Separators = [' ', '-'];

    extension(string str)
    {
        public string Dot(string str2) => str + "." + str2;

        public bool ContainsUppercase() =>
            str.Any(char.IsUpper);

        public bool ContainsLowercase() =>
            str.Any(char.IsLower);

        public bool ContainsNumber() =>
            str.Any(Numbers.Contains);

        public string Capitalize()
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

        public string CapitalizeWord() =>
            str.ToUpper()[0] + str.ToLower()[1..];

        private string CapitalizeEachWord(char separator)
        {
            return string
                .Join(separator, str
                    .Split(separator)
                    .Select(CapitalizeWord));
        }
    }
}