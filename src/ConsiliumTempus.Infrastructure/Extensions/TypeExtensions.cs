namespace ConsiliumTempus.Infrastructure.Extensions;

internal static class TypeExtensions
{
    public static string ToCamelId(this Type type) =>
        type.Name
            .TruncateAggregate()
            .FromPascalToCamelCase()
            .ToId();
}