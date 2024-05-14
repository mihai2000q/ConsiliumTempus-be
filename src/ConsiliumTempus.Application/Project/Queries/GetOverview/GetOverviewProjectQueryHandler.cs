using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetOverview;

public sealed class GetOverviewProjectQueryHandler(IProjectRepository projectRepository)
    : IRequestHandler<GetOverviewProjectQuery, ErrorOr<GetOverviewProjectResult>>
{
    public async Task<ErrorOr<GetOverviewProjectResult>> Handle(GetOverviewProjectQuery query, 
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(ProjectId.Create(query.Id), cancellationToken);
        return project is not null 
            ? new GetOverviewProjectResult(project.Description)
            : Errors.Project.NotFound;
    }
}