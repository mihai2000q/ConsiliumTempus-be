namespace ConsiliumTempus.Application.Common.Extensions;

internal static class StringExtensions
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
            var chars = str.ToLower().ToCharArray();
            var isStartOfWord = true;
            for (var i = 0; i < chars.Length; i++)
            {
                if (isStartOfWord) chars[i] = char.ToUpper(chars[i]);
                isStartOfWord = Separators.Contains(chars[i]);
            }

            return new string(chars);
        }
    }
}