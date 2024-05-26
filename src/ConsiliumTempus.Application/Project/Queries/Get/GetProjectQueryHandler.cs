using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.Get;

public sealed class GetProjectQueryHandler(
    IProjectRepository projectRepository)
    : IRequestHandler<GetProjectQuery, ErrorOr<ProjectAggregate>>
{
    public async Task<ErrorOr<ProjectAggregate>> Handle(GetProjectQuery query, CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(ProjectId.Create(query.Id), cancellationToken);
        return project is null ? Errors.Project.NotFound : project;
    }
}