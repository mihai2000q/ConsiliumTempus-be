using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

/// <summary>
/// Who created and who updated something, and when.
/// It is a part of the <see cref="Interfaces.IAudit"/> entity that holds it, and it is stamped
/// when the changes of that entity are saved, so the domain does not have to create or update it.
/// </summary>
public sealed class Audit : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Audit()
    {
    }

    private Audit(
        UserAggregate? createdBy,
        DateTime createdDateTime,
        UserAggregate? updatedBy,
        DateTime updatedDateTime)
    {
        CreatedBy = createdBy;
        CreatedDateTime = createdDateTime;
        UpdatedBy = updatedBy;
        UpdatedDateTime = updatedDateTime;
    }

    public UserAggregate? CreatedBy { get; }
    public DateTime CreatedDateTime { get; }
    public UserAggregate? UpdatedBy { get; }
    public DateTime UpdatedDateTime { get; }

    public static Audit Create(UserAggregate? createdBy, DateTime createdDateTime)
    {
        return new Audit(createdBy, createdDateTime, createdBy, createdDateTime);
    }

    public Audit Update(UserAggregate? updatedBy, DateTime updatedDateTime)
    {
        return new Audit(CreatedBy, CreatedDateTime, updatedBy, updatedDateTime);
    }

    public Audit Nullify(UserAggregate user)
    {
        return new Audit(
            CreatedBy == user ? null : CreatedBy,
            CreatedDateTime,
            UpdatedBy == user ? null : UpdatedBy,
            UpdatedDateTime);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatedBy;
        yield return CreatedDateTime;
        yield return UpdatedBy;
        yield return UpdatedDateTime;
    }
}