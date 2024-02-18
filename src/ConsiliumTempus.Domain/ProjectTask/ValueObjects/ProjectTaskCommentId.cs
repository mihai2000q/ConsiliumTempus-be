using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.ProjectTask.ValueObjects;

public sealed class ProjectTaskCommentId : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private ProjectTaskCommentId()
    {
    }

    private ProjectTaskCommentId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static ProjectTaskCommentId CreateUnique()
    {
        return new ProjectTaskCommentId(Guid.NewGuid());
    }

    public static ProjectTaskCommentId Create(Guid value)
    {
        return new ProjectTaskCommentId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}