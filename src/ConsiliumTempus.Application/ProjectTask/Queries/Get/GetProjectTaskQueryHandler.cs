using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Queries.Get;

public sealed class GetProjectTaskQueryHandler(IProjectTaskRepository projectTaskRepository)
    : IRequestHandler<GetProjectTaskQuery, ErrorOr<ProjectTaskAggregate>>
{
    public async Task<ErrorOr<ProjectTaskAggregate>> Handle(GetProjectTaskQuery query, CancellationToken cancellationToken)
    {
        var task = await projectTaskRepository.Get(ProjectTaskId.Create(query.Id), cancellationToken);
        return task is not null ? task : Errors.ProjectTask.NotFound;
    }
}