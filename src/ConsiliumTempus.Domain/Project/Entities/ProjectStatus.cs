using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Domain.Project.Entities;

public sealed class ProjectStatus : Entity<ProjectStatusId>, IAudit
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectStatus()
    {
    }

    private ProjectStatus(
        ProjectStatusId id,
        Title title,
        ProjectStatusType status,
        Description description,
        ProjectAggregate project) : base(id)
    {
        Title = title;
        Status = status;
        Description = description;
        Project = project;
    }

    public Title Title { get; private set; } = null!;
    public ProjectStatusType Status { get; private set; }
    public Description Description { get; private set; } = null!;
    public ProjectAggregate Project { get; init; } = null!;
    public Audit Audit { get; init; } = null!;

    public static ProjectStatus Create(
        Title title,
        ProjectStatusType status,
        Description description,
        ProjectAggregate project)
    {
        return new ProjectStatus(
            ProjectStatusId.CreateUnique(),
            title,
            status,
            description,
            project);
    }

    public void Update(
        Title title,
        ProjectStatusType status,
        Description description)
    {
        Title = title;
        Status = status;
        Description = description;
        Project.RefreshActivity();
    }
}