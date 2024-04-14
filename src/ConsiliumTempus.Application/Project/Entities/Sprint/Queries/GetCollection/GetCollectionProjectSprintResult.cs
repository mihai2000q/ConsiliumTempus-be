using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Project.Entities;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectSprintResult(List<ProjectSprint> Sprints);