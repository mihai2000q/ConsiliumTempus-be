using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.User;

public sealed class UserAggregate : AggregateRoot<UserId, Guid>
{
    private UserAggregate()
    {
    }
    
    private UserAggregate(
        UserId id,
        Credentials credentials,
        Name name,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Credentials = credentials;
        Name = name;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<WorkspaceAggregate> _workspaces = [];

    public Credentials Credentials { get; private set; } = default!;
    public Name Name { get; private set; } = default!;
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    public IReadOnlyList<WorkspaceAggregate> Workspaces => _workspaces.AsReadOnly();

    public static UserAggregate Create(
        Credentials credentials,
        Name name)
    {
        return new UserAggregate(
            UserId.CreateUnique(),
            credentials,
            name,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
    
    public static UserAggregate Register(
        Credentials credentials,
        Name name)
    {
        var user = Create(credentials, name);

        user.AddDomainEvent(new UserRegistered(user));
        
        return user;
    }

    public void AddWorkspace(WorkspaceAggregate workspaceAggregate)
    {
        _workspaces.Add(workspaceAggregate);
    }
}