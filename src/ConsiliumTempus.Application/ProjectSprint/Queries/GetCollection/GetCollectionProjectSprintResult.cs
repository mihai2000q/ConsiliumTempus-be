using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.ProjectSprint;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectSprintResult(
    List<ProjectSprintAggregate> Sprints,
    int TotalCount);