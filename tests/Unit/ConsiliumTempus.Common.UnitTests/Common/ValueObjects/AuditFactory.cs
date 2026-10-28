using System.Reflection;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.Common.ValueObjects;

public static class AuditFactory
{
    public static void WithAudit<TEntity>(this TEntity entity, UserAggregate? createdBy = null)
        where TEntity : IAudit
    {
        var audit = Audit.Create(createdBy ?? UserFactory.Create(), DateTime.UtcNow);

        var property = FindAuditProperty(entity.GetType());

        if (property is null)
            throw new InvalidOperationException($"The type '{entity.GetType().Name}' does not have an audit");
        property.SetValue(entity, audit);
    }

    // The audit of the custom field setups is declared on their base class
    private static PropertyInfo? FindAuditProperty(Type? type)
    {
        if (type is null) return null;

        return type.GetProperty(
                   nameof(IAudit.Audit),
                   BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
               ?? FindAuditProperty(type.BaseType);
    }
}