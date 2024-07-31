using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;

public sealed record RemoveAllowedMemberFromProjectCommand(
    Guid Id,
    Guid AllowedMemberId)
    : IRequest<ErrorOr<RemoveAllowedMemberFromProjectResult>>;