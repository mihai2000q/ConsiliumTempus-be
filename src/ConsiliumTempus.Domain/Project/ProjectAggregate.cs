using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.Project.ValueObjects;
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
        WorkspaceAggregate workspace,
        DateTime lastActivity,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Name = name;
        Description = description;
        IsFavorite = isFavorite;
        IsPrivate = isPrivate;
        Workspace = workspace;
        LastActivity = lastActivity;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<ProjectSprint> _sprints = [];

    public Name Name { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public IsFavorite IsFavorite { get; private set; } = default!;
    public IsPrivate IsPrivate { get; private set; } = default!;
    public DateTime LastActivity { get; private set; }
    public WorkspaceAggregate Workspace { get; init; } = default!;
    public IReadOnlyList<ProjectSprint> Sprints => _sprints.AsReadOnly();
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }

    public static ProjectAggregate Create(
        Name name,
        IsPrivate isPrivate,
        WorkspaceAggregate workspace,
        UserAggregate user)
    {
        var project = new ProjectAggregate(
            ProjectId.CreateUnique(),
            name,
            Description.Create(string.Empty), 
            IsFavorite.Create(false),
            isPrivate,
            workspace,
            DateTime.UtcNow,
            DateTime.UtcNow,
            DateTime.UtcNow);

        project.AddDomainEvent(new ProjectCreated(project, user));

        return project;
    }

    public void Update(Name name)
    {
        Name = name;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void AddSprint(ProjectSprint sprint)
    {
        _sprints.Add(sprint);
    }

    public void RefreshActivity()
    {
        LastActivity = DateTime.UtcNow;
        Workspace.RefreshActivity();
    }
}