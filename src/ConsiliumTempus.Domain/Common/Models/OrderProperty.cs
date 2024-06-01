using System.Linq.Expressions;

namespace ConsiliumTempus.Domain.Common.Models;

public abstract record OrderProperty<TEntity>(
    string Identifier,
    Expression<Func<TEntity, object?>> PropertySelector)
{
    public string Identifier { get; } = UpperCamelCaseToSnakeCase(Identifier);

    private static string UpperCamelCaseToSnakeCase(string str)
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