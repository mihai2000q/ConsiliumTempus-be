using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Project;

public sealed class ProjectAggregate : AggregateRoot<ProjectId, Guid>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private ProjectAggregate()
    {
    }
    
    private ProjectAggregate(
        ProjectId id,
        string name,
        string description,
        bool isFavorite,
        bool isPrivate,
        WorkspaceAggregate workspace,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Name = name;
        Description = description;
        IsFavorite = isFavorite;
        IsPrivate = isPrivate;
        Workspace = workspace;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<ProjectSprint> _sprints = [];
    
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsFavorite { get; private set; }
    public bool IsPrivate { get; private set; }
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public WorkspaceAggregate Workspace { get; init; } = default!;
    public IReadOnlyList<ProjectSprint> Sprints => _sprints.AsReadOnly();
    
    public static ProjectAggregate Create(
        string name,
        string description,
        bool isPrivate,
        WorkspaceAggregate workspace)
    {
        return new ProjectAggregate(
            ProjectId.CreateUnique(),
            name,
            description,
            false,
            isPrivate,
            workspace,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }

    public void AddSprint(ProjectSprint sprint)
    {
        _sprints.Add(sprint);
    }
}