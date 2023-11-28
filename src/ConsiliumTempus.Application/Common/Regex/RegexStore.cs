using System.Text.RegularExpressions;

namespace ConsiliumTempus.Application.Common.Regex;

public static partial class RegexStore
{
    [GeneratedRegex("[a-zA-Z0-9._-]+@[a-zA-Z]+\\.+[a-z]+")]
    public static partial System.Text.RegularExpressions.Regex EmailRegex();
}