using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.AddAllowedMember;

public record AddAllowedMemberToProjectCommand(
    Guid Id,
    Guid AllowedMemberId)
    : IRequest<ErrorOr<AddAllowedMemberToProjectResult>>;