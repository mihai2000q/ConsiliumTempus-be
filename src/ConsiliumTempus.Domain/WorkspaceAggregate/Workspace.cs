using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.UserAggregate;
using ConsiliumTempus.Domain.WorkspaceAggregate.ValueObjects;

namespace ConsiliumTempus.Domain.WorkspaceAggregate;

public sealed class Workspace : AggregateRoot<WorkspaceId, Guid>
{
    private Workspace()
    {
    }

    private Workspace(
        WorkspaceId id,
        string name,
        string description) : base(id)
    {
        Name = name;
        Description = description;
    }
    
    private readonly List<User> _users = []; 
    
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public IReadOnlyList<User> Users => _users.AsReadOnly();

    public static Workspace Create(
        string name,
        string description)
    {   
        var workspace = new Workspace(
            WorkspaceId.CreateUnique(),
            name,
            description);

        //workspace.AddUser(user);
        
        return workspace;
    }

    private void AddUser(User user)
    {
        _users.Add(user);
    }
}