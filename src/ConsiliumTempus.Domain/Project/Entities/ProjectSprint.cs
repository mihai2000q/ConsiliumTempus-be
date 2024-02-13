using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Domain.Project.Entities;

public sealed class ProjectSprint : Entity<ProjectSprintId>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private ProjectSprint()
    {
    }

    private ProjectSprint(
        string name,
        ProjectAggregate project,
        DateTime createdDateTime,
        DateTime updatedDateTime)
    {
        Name = name;
        Project = project;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<ProjectSection> _sections = [];

    public string Name { get; private set; } = string.Empty;
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public ProjectAggregate Project { get; init; } = default!;
    public IReadOnlyList<ProjectSection> Sections => _sections.AsReadOnly();

    public static ProjectSprint Create(
        string name,
        ProjectAggregate project)
    {
        return new ProjectSprint(
            name,
            project,
            DateTime.UtcNow, 
            DateTime.UtcNow);
    }

    public void AddSection(ProjectSection section)
    {
        _sections.Add(section);
    }
}