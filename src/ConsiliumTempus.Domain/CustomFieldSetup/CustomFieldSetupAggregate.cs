using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup;

public abstract class CustomFieldSetupAggregate : AggregateRoot<CustomFieldSetupId>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    protected CustomFieldSetupAggregate()
    {
    }

    protected CustomFieldSetupAggregate(
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        Audit audit) : base(id)
    {
        Name = name;
        Description = description;
        Workspace = workspace;
        Project = project;
        Audit = audit;
    }

    public Name Name { get; protected set; } = default!;
    public Description Description { get; protected set; } = default!;
    public WorkspaceAggregate? Workspace { get; init; }
    public ProjectAggregate? Project { get; init; }
    public Audit Audit { get; init; } = null!;

    protected void Update(
        Name name,
        Description description,
        UserAggregate updatedBy)
    {
        Name = name;
        Description = description;
        Audit.Update(updatedBy);
    }
}