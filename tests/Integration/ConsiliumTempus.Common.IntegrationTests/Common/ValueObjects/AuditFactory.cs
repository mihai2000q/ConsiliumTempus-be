using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.IntegrationTests.Common.ValueObjects;

public static class AuditFactory
{
    public static Audit Create(
        UserAggregate createdBy,
        UserAggregate? updatedBy = null,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        return EntityBuilder<Audit>.Empty()
            .WithProperty(nameof(Audit.CreatedBy), createdBy)
            .WithProperty(nameof(Audit.UpdatedBy), updatedBy ?? createdBy)
            .WithProperty(nameof(Audit.CreatedDateTime), createdDateTime ?? DateTime.UtcNow)
            .WithProperty(nameof(Audit.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow)
            .Build();
    }
}