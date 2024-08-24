using System.Reflection;

namespace ConsiliumTempus.Common.IntegrationTests;

internal sealed class EntityBuilder<TEntity>
    where TEntity : class
{
    private TEntity Entity { get; }

    private EntityBuilder(TEntity entity)
    {
        Entity = entity;
    }

    public static EntityBuilder<TEntity> Empty()
    {
        var constructor = GetDefaultPrivateConstructor();
        var obj = constructor!.Invoke([]) as TEntity;
        return new EntityBuilder<TEntity>(obj!);
    }

    public EntityBuilder<TEntity> WithProperty(string propertyName, object? newProperty)
    {
        var properties = typeof(TEntity).GetProperties();
        var propertyInfo = properties.SingleOrDefault(p => 
                               p.Name == propertyName &&
                               p.DeclaringType == typeof(TEntity))
                           ?? properties.FirstOrDefault(p => p.Name == propertyName && p.CanWrite)
                           ?? properties.First(p => p.Name == propertyName);
        if (propertyInfo.CanWrite)
            propertyInfo.SetValue(Entity, newProperty);
        else
            propertyInfo.DeclaringType?.GetRuntimeFields()
                .SingleOrDefault(f => f.Name == ToObjectBackingField(propertyName))
                ?.SetValue(Entity, newProperty);

        return this;
    }

    public EntityBuilder<TEntity> WithField(string fieldName, object? newField)
    {
        var field = typeof(TEntity).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

        if (field is not null) field.SetValue(Entity, newField);
        else if (typeof(TEntity).BaseType is not null) SetParentField(fieldName, newField);

        return this;
    }

    public TEntity Build()
    {
        return Entity;
    }

    private static ConstructorInfo? GetDefaultPrivateConstructor()
    {
        return typeof(TEntity).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            Type.EmptyTypes,
            null);
    }

    private static string ToObjectBackingField(string propertyName) =>
        $"<{propertyName}>k__BackingField";

    private void SetParentField(string fieldName, object? newField)
    {
        typeof(TEntity).BaseType
            !.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(Entity, newField);
    }
}