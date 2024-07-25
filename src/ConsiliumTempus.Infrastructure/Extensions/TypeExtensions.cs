namespace ConsiliumTempus.Infrastructure.Extensions;

public static class TypeExtensions
{
    public static string ToCamelId(this Type type) =>
        type.Name
            .TruncateAggregate()
            .FromPascalToCamelCase()
            .ToId();
}