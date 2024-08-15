using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public abstract class CustomField : Entity<CustomFieldId>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    protected CustomField()
    {
    }

    protected CustomField(CustomFieldId id, ProjectTaskAggregate task) : base(id)
    {
        Task = task;
    }

    public ProjectTaskAggregate Task { get; init; } = null!;
}