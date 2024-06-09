using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.Get;

public sealed class GetProjectQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository projectRepository)
    : IRequestHandler<GetProjectQuery, ErrorOr<GetProjectResult>>
{
    public async Task<ErrorOr<GetProjectResult>> Handle(GetProjectQuery query, CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(ProjectId.Create(query.Id), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var currentUser = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        return new GetProjectResult(project, currentUser);
    }
}