using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;

public sealed class GetCollectionProjectSprintQueryHandler(
    IProjectRepository projectRepository,
    IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<GetCollectionProjectSprintQuery, ErrorOr<GetCollectionProjectSprintResult>>
{
    public async Task<ErrorOr<GetCollectionProjectSprintResult>> Handle(
        GetCollectionProjectSprintQuery query, 
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(ProjectId.Create(query.ProjectId), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        return new GetCollectionProjectSprintResult(
            await projectSprintRepository.GetListByProject(project.Id, cancellationToken));
    }
}