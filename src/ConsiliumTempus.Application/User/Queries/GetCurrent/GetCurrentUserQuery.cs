using ConsiliumTempus.Domain.User;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Queries.GetCurrent;

public sealed record GetCurrentUserQuery: IRequest<ErrorOr<UserAggregate>>;