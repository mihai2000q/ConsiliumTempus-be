using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.Common.Entities;

public sealed class Audit : Entity<Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Audit()
    {
    }

    private Audit(
        Guid id, 
        UserAggregate? createdBy,
        DateTime createdDateTime,
        UserAggregate? updatedBy,
        DateTime updatedDateTime) : base(id)
    {
        CreatedBy = createdBy;
        CreatedDateTime = createdDateTime;
        UpdatedBy = updatedBy;
        UpdatedDateTime = updatedDateTime;
    }

    public UserAggregate? CreatedBy { get; private set; }
    public DateTime CreatedDateTime { get; init; }
    public UserAggregate? UpdatedBy { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    public static Audit Create(UserAggregate? createdBy)
    {
        return new Audit(
            Guid.NewGuid(),
            createdBy,
            DateTime.UtcNow,
            createdBy,
            DateTime.UtcNow);
    }

    public void Update(UserAggregate updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDateTime = DateTime.UtcNow;
    }
    
    public void Nullify()
    {
        CreatedBy = null;
        UpdatedBy = null;
    }
}