using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Domain.Workspace;

public sealed class WorkspaceAggregate : AggregateRoot<WorkspaceId, Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private WorkspaceAggregate()
    {
    }

    private WorkspaceAggregate(
        WorkspaceId id,
        Name name,
        Description description,
        UserAggregate owner,
        IsUserWorkspace isUserWorkspace,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Name = name;
        Description = description;
        Owner = owner;
        IsUserWorkspace = isUserWorkspace;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<Membership> _memberships = [];
    private readonly List<ProjectAggregate> _projects = [];

    public Name Name { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public IsUserWorkspace IsUserWorkspace { get; private set; } = default!;
    public UserAggregate Owner { get; private set; } = default!;
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    public IReadOnlyList<Membership> Memberships => _memberships.AsReadOnly();
    public IReadOnlyList<ProjectAggregate> Projects => _projects.AsReadOnly();

    public static WorkspaceAggregate Create(
        Name name,
        Description description,
        UserAggregate owner,
        IsUserWorkspace isUserWorkspace)
    {
        var workspace = new WorkspaceAggregate(
            WorkspaceId.CreateUnique(),
            name,
            description,
            owner,
            isUserWorkspace,
            DateTime.UtcNow,
            DateTime.UtcNow);

        return workspace;
    }

    public void Update(Name name, Description description)
    {
        Name = name;
        Description = description;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void RefreshUpdatedDateTime()
    {
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void AddUserMembership(Membership membership)
    {
        _memberships.Add(membership);
    }

    public void TransferOwnership(UserAggregate owner)
    {
        Owner = owner;
    }
    
    public void UpdateIsUserWorkspace(IsUserWorkspace isUserWorkspace)
    {
        IsUserWorkspace = isUserWorkspace;
    }

}