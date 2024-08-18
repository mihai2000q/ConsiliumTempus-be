using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
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
        Audit audit) : base(id)
    {
        Name = name;
        Description = description;
        Workspace = workspace;
        Audit = audit;
    }

    private readonly List<ProjectAggregate> _projects = [];

    public Name Name { get; protected set; } = default!;
    public Description Description { get; protected set; } = default!;
    public WorkspaceAggregate? Workspace { get; protected set; }
    public IReadOnlyList<ProjectAggregate> Projects => _projects.AsReadOnly();
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
    
    public void UpdateWorkspace(WorkspaceAggregate workspace, UserAggregate updatedBy)
    {
        Workspace = workspace;
        Audit.Update(updatedBy);
    }

    public void AddProject(ProjectAggregate project)
    {
        _projects.Add(project);
        AddDomainEvent(new AddedCustomFieldSetupToProject(this, project));
    }

    public void RemoveProject(ProjectAggregate project)
    {
        _projects.Remove(project);
    }
}