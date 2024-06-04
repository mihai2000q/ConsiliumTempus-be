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
        var audit = DomainFactory.GetObjectInstance<Audit>();

        DomainFactory.SetProperty(ref audit, nameof(audit.Id), Guid.NewGuid());
        DomainFactory.SetProperty(ref audit, nameof(audit.CreatedBy), createdBy);
        DomainFactory.SetProperty(ref audit, nameof(audit.UpdatedBy), updatedBy);
        DomainFactory.SetProperty(ref audit, nameof(audit.CreatedDateTime), createdDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref audit, nameof(audit.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow);

        return audit;
    }
}