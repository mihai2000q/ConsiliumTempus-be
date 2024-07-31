using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;

public sealed record UpdatePrivacyProjectCommand(
    Guid Id,
    bool IsPrivate)
    : IRequest<ErrorOr<UpdatePrivacyProjectResult>>;