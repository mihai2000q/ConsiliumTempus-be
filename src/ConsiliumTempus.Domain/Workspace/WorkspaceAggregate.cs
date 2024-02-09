using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Models;
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
        string description,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Name = name;
        Description = description;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }
    
    private readonly List<Membership> _memberships = []; 
    
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    public IReadOnlyList<Membership> Memberships => _memberships.AsReadOnly();

    public static WorkspaceAggregate Create(
        string name,
        string description)
    {   
        var workspace = new WorkspaceAggregate(
            WorkspaceId.CreateUnique(),
            name,
            description,
            DateTime.UtcNow, 
            DateTime.UtcNow);
        
        return workspace;
    }

    public void AddUserMembership(Membership membership)
    {
        _memberships.Add(membership);
    }
}