using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;

public sealed record RemoveCustomFieldSetupFromProjectCommand(
    Guid Id,
    Guid ProjectId) 
    : IRequest<ErrorOr<RemoveCustomFieldSetupFromProjectResult>>;