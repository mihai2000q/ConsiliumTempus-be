using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.Get;

public sealed class GetProjectSprintQueryHandler(IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<GetProjectSprintQuery, ErrorOr<ProjectSprintAggregate>>
{
    public async Task<ErrorOr<ProjectSprintAggregate>> Handle(GetProjectSprintQuery query, CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.Get(ProjectSprintId.Create(query.Id), cancellationToken);
        return sprint is null ? Errors.ProjectSprint.NotFound : sprint;
    }
}