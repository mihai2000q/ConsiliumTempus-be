using ConsiliumTempus.Domain.Project.Entities;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Queries.Get;

public sealed record GetProjectSprintQuery(Guid Id) : IRequest<ErrorOr<ProjectSprint>>;