using System.Reflection;

namespace ConsiliumTempus.Api.IntegrationTests.TestFactory;

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
                .Single(f => f.Name == GetBackingFieldName(propertyName))
                .SetValue(obj, newProperty); 
        }
    }
    
    private static ConstructorInfo? GetDefaultPrivateConstructor<T>()
    {
        return typeof(T).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            Type.EmptyTypes,
            null);
    }
    
    private const string Prefix = "<";
    private const string Suffix = ">k__BackingField";

    private static string GetBackingFieldName(string propertyName) => $"{Prefix}{propertyName}{Suffix}";
}