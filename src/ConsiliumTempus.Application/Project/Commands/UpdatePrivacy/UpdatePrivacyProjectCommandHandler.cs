using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;

public sealed class UpdatePrivacyProjectCommandHandler(IProjectRepository projectRepository)
    : IRequestHandler<UpdatePrivacyProjectCommand, ErrorOr<UpdatePrivacyProjectResult>>
{
    public async Task<ErrorOr<UpdatePrivacyProjectResult>> Handle(UpdatePrivacyProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(ProjectId.Create(command.Id), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        project.UpdatePrivacy(IsPrivate.Create(command.IsPrivate));

        return new UpdatePrivacyProjectResult();
    }
}