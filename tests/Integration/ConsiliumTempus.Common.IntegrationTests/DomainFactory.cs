using System.Reflection;

namespace ConsiliumTempus.Common.IntegrationTests;

internal static class DomainFactory
{
    internal static T GetObjectInstance<T>() where T : class
    {
        var constructor = GetDefaultPrivateConstructor<T>();
        var obj = constructor!.Invoke([]) as T;
        return obj!;
    }

    internal static void SetProperty<T>(ref T obj, string propertyName, object? newProperty)
    {
        var propertyInfo = typeof(T).GetProperty(propertyName)!;
        if (propertyInfo.CanWrite)
        {
            propertyInfo.SetValue(obj, newProperty);
        }
        else
        {
            propertyInfo.DeclaringType?.GetRuntimeFields()
                .SingleOrDefault(f => f.Name == propertyName.ToObjectBackingField())
                ?.SetValue(obj, newProperty);
        }
    }

    internal static void SetField<T>(ref T obj, string fieldName, object? newField)
    {
        typeof(T)
            .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(obj, newField);
    }

    private static ConstructorInfo? GetDefaultPrivateConstructor<T>()
    {
        return typeof(T).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            Type.EmptyTypes,
            null);
    }

    private static string ToObjectBackingField(this string propertyName) =>
        $"<{propertyName}>k__BackingField";
}