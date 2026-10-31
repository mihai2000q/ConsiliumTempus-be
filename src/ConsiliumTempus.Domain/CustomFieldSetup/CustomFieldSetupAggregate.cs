using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup;

public abstract class CustomFieldSetupAggregate : AggregateRoot<CustomFieldSetupId>, IAudit
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    protected CustomFieldSetupAggregate()
    {
    }

    protected CustomFieldSetupAggregate(
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace) : base(id)
    {
        Name = name;
        Description = description;
        Workspace = workspace;
    }

    private readonly List<ProjectAggregate> _projects = [];

    public Name Name { get; protected set; } = null!;
    public Description Description { get; protected set; } = null!;
    public WorkspaceAggregate? Workspace { get; protected set; }
    public IReadOnlyList<ProjectAggregate> Projects => _projects.AsReadOnly();
    public Audit Audit { get; init; } = null!;

    public abstract CustomFieldType Type { get; }
    public bool IsGlobal => Workspace is not null;
    public WorkspaceAggregate OwningWorkspace => Workspace ?? Projects.Single().Workspace;
    public ProjectAggregate? OwningProject => IsGlobal ? null : Projects.Single();

    protected void Update(
        Name name,
        Description description)
    {
        Name = name;
        Description = description;
    }
    
    public void UpdateWorkspace(WorkspaceAggregate workspace)
    {
        Workspace = workspace;
    }

    public void AddProject(ProjectAggregate project)
    {
        _projects.Add(project);
        AddDomainEvent(new AddedCustomFieldSetupToProject(this, project));
    }

    public void RemoveProject(ProjectAggregate project)
    {
        if (_projects.Remove(project)) 
            AddDomainEvent(new RemovedCustomFieldSetupFromProject(this, project));
    }
}