using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.IntegrationTests.Common.Entities;

public static class AuditFactory
{
    public static Audit Create(
        UserAggregate createdBy,
        UserAggregate updatedBy,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        return EntityBuilder<Audit>.Empty()
            .WithProperty(nameof(Audit.Id), Guid.NewGuid())
            .WithProperty(nameof(Audit.CreatedBy), createdBy)
            .WithProperty(nameof(Audit.UpdatedBy), updatedBy)
            .WithProperty(nameof(Audit.CreatedDateTime), createdDateTime ?? DateTime.UtcNow)
            .WithProperty(nameof(Audit.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow)
            .Build();
    }
}