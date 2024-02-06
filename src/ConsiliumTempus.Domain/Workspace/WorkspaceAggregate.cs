using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
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
        string name,
        string description) : base(id)
    {
        Name = name;
        Description = description;
    }
    
    private readonly List<UserAggregate> _users = []; 
    
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public IReadOnlyList<UserAggregate> Users => _users.AsReadOnly();

    public static WorkspaceAggregate Create(
        string name,
        string description)
    {   
        var workspace = new WorkspaceAggregate(
            WorkspaceId.CreateUnique(),
            name,
            description);
        
        return workspace;
    }

    public void AddUser(UserAggregate userAggregate)
    {
        _users.Add(userAggregate);
    }
}