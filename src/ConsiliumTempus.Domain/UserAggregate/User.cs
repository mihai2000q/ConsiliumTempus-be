using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.UserAggregate.Events;
using ConsiliumTempus.Domain.UserAggregate.ValueObjects;
using ConsiliumTempus.Domain.WorkspaceAggregate;

namespace ConsiliumTempus.Domain.UserAggregate;

public sealed class User : AggregateRoot<UserId, Guid>
{
    private User()
    {
    }
    
    private User(
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

    private readonly List<Workspace> _workspaces = [];

    public Credentials Credentials { get; private set; } = default!;
    public Name Name { get; private set; } = default!;
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    public IReadOnlyList<Workspace> Workspaces => _workspaces.AsReadOnly();

    public static User Create(
        Credentials credentials,
        Name name)
    {
        return new User(
            UserId.CreateUnique(),
            credentials,
            name,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
    
    public static User Register(
        Credentials credentials,
        Name name)
    {
        var user = Create(credentials, name);

        user.AddDomainEvent(new UserRegistered(user));
        
        return user;
    }

    public void AddWorkspace(Workspace workspace)
    {
        _workspaces.Add(workspace);
    }
}