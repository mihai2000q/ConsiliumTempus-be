using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.Project.Entities;

public sealed class ProjectTask : Entity<ProjectTaskId>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private ProjectTask()
    {
    }

    private ProjectTask(
        string name,
        string description,
        ProjectSection section,
        DateTime createdDateTime,
        DateTime updatedDateTime)
    {
        Name = name;
        Description = description;
        Section = section;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }
    
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public UserAggregate? Asignee { get; private set; } = null;
    public UserAggregate? Reviewer { get; private set; } = null;
    public DateOnly? DueDate { get; private set; } = null;
    public ProjectSection Section { get; init; } = default!;

    public static ProjectTask Create(
        string name,
        string description,
        ProjectSection section)
    {
        return new ProjectTask(
            name,
            description,
            section,
            DateTime.UtcNow, 
            DateTime.UtcNow);
    }
}