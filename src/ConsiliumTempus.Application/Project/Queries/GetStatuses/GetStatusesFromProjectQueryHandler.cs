using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetStatuses;

public sealed class GetStatusesFromProjectQueryHandler(IProjectRepository projectRepository)
    : IRequestHandler<GetStatusesFromProjectQuery, ErrorOr<GetStatusesFromProjectResult>>
{
    public async Task<ErrorOr<GetStatusesFromProjectResult>> Handle(GetStatusesFromProjectQuery query, 
        CancellationToken cancellationToken)
    {
        var projectId = ProjectId.Create(query.Id);
        var statuses = await projectRepository.GetStatuses(projectId, cancellationToken);
        var totalCount = await projectRepository.GetStatusesCount(projectId, cancellationToken);

        return new GetStatusesFromProjectResult(statuses, totalCount);
    }
}