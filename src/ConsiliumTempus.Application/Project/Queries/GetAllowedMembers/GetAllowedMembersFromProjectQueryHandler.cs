using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetAllowedMembers;

public sealed class GetAllowedMembersFromProjectQueryHandler(IProjectRepository projectRepository)
    : IRequestHandler<GetAllowedMembersFromProjectQuery, ErrorOr<GetAllowedMembersFromProjectResult>>
{
    public async Task<ErrorOr<GetAllowedMembersFromProjectResult>> Handle(GetAllowedMembersFromProjectQuery query, 
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithAllowedMembers(ProjectId.Create(query.Id), cancellationToken);
        return project is not null
            ? new GetAllowedMembersFromProjectResult(project.AllowedMembers)
            : Errors.Project.NotFound;
    }
}