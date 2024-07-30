using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.AddAllowedMember;

public sealed record AddAllowedMemberToProjectCommand(
    Guid Id,
    Guid CollaboratorId)
    : IRequest<ErrorOr<AddAllowedMemberToProjectResult>>;