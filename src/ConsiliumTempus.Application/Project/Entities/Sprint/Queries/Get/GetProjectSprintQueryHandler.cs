using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Queries.Get;

public sealed class GetProjectSprintQueryHandler(IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<GetProjectSprintQuery, ErrorOr<ProjectSprint>>
{
    public async Task<ErrorOr<ProjectSprint>> Handle(GetProjectSprintQuery query, CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.Get(ProjectSprintId.Create(query.Id), cancellationToken);
        return sprint is null ? Errors.ProjectSprint.NotFound : sprint;
    }
}