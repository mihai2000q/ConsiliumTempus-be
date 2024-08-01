using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Domain.Workspace.Events;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Domain.Workspace;

public sealed class WorkspaceAggregate : AggregateRoot<WorkspaceId, Guid>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private WorkspaceAggregate()
    {
    }

    private WorkspaceAggregate(
        WorkspaceId id,
        Name name,
        Description description,
        UserAggregate owner,
        IsPersonal isPersonal,
        DateTime lastActivity,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Name = name;
        Description = description;
        Owner = owner;
        IsPersonal = isPersonal;
        LastActivity = lastActivity;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<Membership> _memberships = [];
    private readonly List<ProjectAggregate> _projects = [];
    private readonly List<UserAggregate> _favorites = [];
    private readonly List<WorkspaceInvitation> _invitations = [];

    public Name Name { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public IsPersonal IsPersonal { get; private set; } = default!;
    public UserAggregate Owner { get; private set; } = default!;
    public DateTime LastActivity { get; private set; }
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public IReadOnlyList<Membership> Memberships => _memberships.AsReadOnly();
    public IReadOnlyList<ProjectAggregate> Projects => _projects.AsReadOnly();
    public IReadOnlyList<UserAggregate> Favorites => _favorites.AsReadOnly();
    public IReadOnlyList<WorkspaceInvitation> Invitations => _invitations.AsReadOnly();

    public static WorkspaceAggregate Create(
        Name name,
        Description description,
        UserAggregate owner,
        IsPersonal isPersonal)
    {
        var workspace = new WorkspaceAggregate(
            WorkspaceId.CreateUnique(),
            name,
            description, 
            owner,
            isPersonal,
            DateTime.UtcNow,
            DateTime.UtcNow,
            DateTime.UtcNow);

        var membership = Membership.Create(owner, workspace, WorkspaceRole.Admin);
        workspace.AddUserMembership(membership);

        return workspace;
    }
    
    public bool IsFavorite(UserAggregate user)
    {
        return _favorites.Contains(user);
    }

    public void Update(Name name)
    {
        Name = name;
        UpdatedDateTime = DateTime.UtcNow;
        LastActivity = DateTime.UtcNow;
    }

    public void UpdateFavorites(bool isFavorite, UserAggregate user)
    {
        if (isFavorite)
            _favorites.Add(user);
        else
            _favorites.Remove(user);
    }

    public void UpdateOverview(Description description)
    {
        Description = description;
        UpdatedDateTime = DateTime.UtcNow;
        LastActivity = DateTime.UtcNow;
    }

    public void UpdateOwner(UserAggregate owner)
    {
        Owner = owner;
    }

    public void UpdateIsPersonal(IsPersonal isPersonal)
    {
        IsPersonal = isPersonal;
    }

    public void RefreshUpdatedDateTime()
    {
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void RefreshActivity()
    {
        LastActivity = DateTime.UtcNow;
    }

    public void AddUserMembership(Membership membership)
    {
        _memberships.Add(membership);
    }

    public void RemoveUserMembership(Membership membership)
    {
        _memberships.Remove(membership);
        AddDomainEvent(new CollaboratorRemovedFromWorkspace(this, membership.User));
    }

    public void AddInvitation(WorkspaceInvitation invitation)
    {
        _invitations.Add(invitation);
    }

    public void RemoveInvitation(WorkspaceInvitation invitation)
    {
        _invitations.Remove(invitation);
    }

    public UserAggregate NextProjectOwner()
    {
        return Memberships
            .OrderBy(m => m.User == Owner)
            .ThenByDescending(m => m.WorkspaceRole.Id)
            .First()
            .User;
    }
}