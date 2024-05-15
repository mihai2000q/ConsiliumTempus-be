using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdateOverview;

public sealed class UpdateOverviewProjectCommandHandler(IProjectRepository projectRepository)
    : IRequestHandler<UpdateOverviewProjectCommand, ErrorOr<UpdateOverviewProjectResult>>
{
    public async Task<ErrorOr<UpdateOverviewProjectResult>> Handle(UpdateOverviewProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithWorkspace(ProjectId.Create(command.Id), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        project.UpdateOverview(
            Description.Create(command.Description));
        
        return new UpdateOverviewProjectResult();
    }
}