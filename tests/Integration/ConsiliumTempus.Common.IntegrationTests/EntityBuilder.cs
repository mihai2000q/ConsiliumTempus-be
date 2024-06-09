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
        var propertyInfo = typeof(TEntity).GetProperty(propertyName)!;
        if (propertyInfo.CanWrite)
        {
            propertyInfo.SetValue(Entity, newProperty);
        }
        else
        {
            propertyInfo.DeclaringType?.GetRuntimeFields()
                .SingleOrDefault(f => f.Name == ToObjectBackingField(propertyName))
                ?.SetValue(Entity, newProperty);
        }

        return this;
    }

    public EntityBuilder<TEntity> WithField(string fieldName, object? newField)
    {
        typeof(TEntity)
            .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(Entity, newField);

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
}