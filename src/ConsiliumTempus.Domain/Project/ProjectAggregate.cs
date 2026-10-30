using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Project;

public sealed class ProjectAggregate : AggregateRoot<ProjectId>, IAudit
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectAggregate()
    {
    }

    private ProjectAggregate(
        ProjectId id,
        Name name,
        Description description,
        IsPrivate isPrivate,
        UserAggregate owner,
        ProjectLifecycle lifecycle,
        WorkspaceAggregate workspace,
        DateTime lastActivity) : base(id)
    {
        Name = name;
        Description = description;
        IsPrivate = isPrivate;
        Owner = owner;
        Lifecycle = lifecycle;
        Workspace = workspace;
        LastActivity = lastActivity;
    }

    private readonly List<ProjectSprintAggregate> _sprints = [];
    private readonly List<ProjectStatus> _statuses = [];
    private readonly List<UserAggregate> _favorites = [];
    private readonly List<UserAggregate> _allowedMembers = [];
    private readonly List<CustomFieldSetupAggregate> _customFieldSetups = [];

    public Name Name { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public IsPrivate IsPrivate { get; private set; } = null!;
    public UserAggregate Owner { get; private set; } = null!;
    public ProjectLifecycle Lifecycle { get; private set; }
    public DateTime LastActivity { get; private set; }
    public WorkspaceAggregate Workspace { get; init; } = null!;
    public ProjectStatus? LatestStatus => _statuses.Count == 0 ? null : _statuses[0];
    public IReadOnlyList<ProjectSprintAggregate> Sprints => _sprints.AsReadOnly();
    public IReadOnlyList<ProjectStatus> Statuses => _statuses.AsReadOnly();
    public IReadOnlyList<UserAggregate> Favorites => _favorites.AsReadOnly();
    public IReadOnlyList<UserAggregate> AllowedMembers => _allowedMembers.AsReadOnly();
    public IReadOnlyList<CustomFieldSetupAggregate> CustomFieldSetups => _customFieldSetups.AsReadOnly();
    public Audit Audit { get; init; } = null!;

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
            isPrivate,
            owner,
            ProjectLifecycle.Active,
            workspace,
            DateTime.UtcNow);

        project.AddAllowedMember(owner);

        project.AddDomainEvent(new ProjectCreated(project));

        return project;
    }

    public bool IsFavorite(UserAggregate user)
    {
        return _favorites.Contains(user);
    }

    public void Update(Name name, ProjectLifecycle lifecycle)
    {
        Name = name;
        Lifecycle = lifecycle;
        RefreshActivity();
    }

    public void UpdateFavorites(bool isFavorite, UserAggregate user)
    {
        if (isFavorite)
            _favorites.Add(user);
        else
            _favorites.Remove(user);
    }

    public void UpdatePrivacy(IsPrivate isPrivate)
    {
        IsPrivate = isPrivate;
        RefreshActivity();
    }

    public void UpdateOwner(UserAggregate owner)
    {
        Owner = owner;
        // the owner is always an allowed member
        if (!_allowedMembers.Contains(owner)) AddAllowedMember(owner);
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
        _statuses.Insert(0, status);
    }

    public void RemoveStatus(ProjectStatus status)
    {
        _statuses.Remove(status);
    }

    public void AddAllowedMember(UserAggregate member)
    {
        _allowedMembers.Add(member);
    }

    public void RemoveAllowedMember(UserAggregate member)
    {
        _allowedMembers.Remove(member);
        AddDomainEvent(new AllowedMemberRemovedFromProject(this, member));
    }
}