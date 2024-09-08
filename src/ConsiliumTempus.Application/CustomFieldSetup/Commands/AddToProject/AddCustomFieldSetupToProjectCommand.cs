using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;

public sealed record AddCustomFieldSetupToProjectCommand(
    Guid Id,
    Guid ProjectId)
    : IRequest<ErrorOr<AddCustomFieldSetupToProjectResult>>;