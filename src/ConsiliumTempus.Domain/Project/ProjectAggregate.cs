using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Project;

public sealed class ProjectAggregate : AggregateRoot<ProjectId, Guid>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectAggregate()
    {
    }

    private ProjectAggregate(
        ProjectId id,
        Name name,
        Description description,
        IsFavorite isFavorite,
        IsPrivate isPrivate,
        UserAggregate owner,
        ProjectLifecycle lifecycle,
        WorkspaceAggregate workspace,
        DateTime lastActivity,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Name = name;
        Description = description;
        IsFavorite = isFavorite;
        IsPrivate = isPrivate;
        Owner = owner;
        Lifecycle = lifecycle;
        Workspace = workspace;
        LastActivity = lastActivity;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<ProjectSprintAggregate> _sprints = [];
    private readonly List<ProjectStatus> _status = [];

    public Name Name { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public IsFavorite IsFavorite { get; private set; } = default!;
    public IsPrivate IsPrivate { get; private set; } = default!;
    public UserAggregate Owner { get; private set; } = default!;
    public ProjectLifecycle Lifecycle { get; private set; }
    public DateTime LastActivity { get; private set; }
    public WorkspaceAggregate Workspace { get; init; } = default!;
    public IReadOnlyList<ProjectSprintAggregate> Sprints => _sprints.AsReadOnly();
    public IReadOnlyList<ProjectStatus> Statuses => _status.AsReadOnly();
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }

    public static ProjectAggregate Create(
        Name name,
        IsPrivate isPrivate,
        WorkspaceAggregate workspace,
        UserAggregate owner)
    {
        var project = new ProjectAggregate(
            ProjectId.CreateUnique(),
            name,
            Description.Create(string.Empty),
            IsFavorite.Create(false),
            isPrivate,
            owner,
            ProjectLifecycle.Active,
            workspace,
            DateTime.UtcNow,
            DateTime.UtcNow,
            DateTime.UtcNow);

        project.AddDomainEvent(new ProjectCreated(project));

        return project;
    }

    public void Update(Name name, IsFavorite isFavorite)
    {
        Name = name;
        IsFavorite = isFavorite;
        UpdatedDateTime = DateTime.UtcNow;
        RefreshActivity();
    }

    public void UpdateOverview(Description description)
    {
        Description = description;
        RefreshActivity();
    }

    public void RefreshActivity()
    {
        LastActivity = DateTime.UtcNow;
        Workspace.RefreshActivity();
    }

    public void AddSprint(ProjectSprintAggregate sprint)
    {
        _sprints.Add(sprint);
    }

    public void AddStatus(ProjectStatus status)
    {
        _status.Add(status);
    }

    public void RemoveStatus(ProjectStatus status)
    {
        _status.Remove(status);
    }
}