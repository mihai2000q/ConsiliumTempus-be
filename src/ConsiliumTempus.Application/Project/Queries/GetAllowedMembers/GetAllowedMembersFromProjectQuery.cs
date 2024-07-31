using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetAllowedMembers;

public sealed record GetAllowedMembersFromProjectQuery(Guid Id) : IRequest<ErrorOr<GetAllowedMembersFromProjectResult>>;