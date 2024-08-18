using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;

public sealed record UpdateWorkspaceCustomFieldSetupCommand(
    Guid Id,
    Guid WorkspaceId) 
    : IRequest<ErrorOr<UpdateWorkspaceCustomFieldSetupResult>>;