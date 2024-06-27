using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetStages;

public sealed class GetStagesFromProjectSprintQueryHandler(IProjectSprintRepository projectSprintRepository) 
    : IRequestHandler<GetStagesFromProjectSprintQuery, ErrorOr<GetStagesFromProjectSprintResult>>
{
    public async Task<ErrorOr<GetStagesFromProjectSprintResult>> Handle(GetStagesFromProjectSprintQuery query, 
        CancellationToken cancellationToken)
    {
        var stages = await projectSprintRepository.GetStages(
            ProjectSprintId.Create(query.Id), 
            cancellationToken);
        return new GetStagesFromProjectSprintResult(stages);
    }
}