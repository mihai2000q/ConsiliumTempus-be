using System.Reflection;

namespace ConsiliumTempus.Domain.Common.Models;

public abstract class Enumeration<TEnum> : Entity<int>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

    protected Enumeration()
    {
    }

    protected Enumeration(int id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; } = string.Empty;

    public static TEnum? FromValue(int value)
    {
        return Enumerations.GetValueOrDefault(value);
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations
            .Values
            .SingleOrDefault(e => e.Name == name);
    }

    public static TEnum[] GetValues()
    {
        return Enumerations.Values.ToArray();
    }

    public override string ToString()
    {
        return Name;
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var fieldsForType = enumerationType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Id);
    }
}