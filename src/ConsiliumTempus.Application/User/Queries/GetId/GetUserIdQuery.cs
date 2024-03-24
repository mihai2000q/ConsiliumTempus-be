using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Queries.GetId;

public sealed record GetUserIdQuery: IRequest<ErrorOr<UserId>>;