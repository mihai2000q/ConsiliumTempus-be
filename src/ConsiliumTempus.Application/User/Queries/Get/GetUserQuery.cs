using ConsiliumTempus.Domain.User;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Queries.Get;

public sealed record GetUserQuery(Guid Id) : IRequest<ErrorOr<UserAggregate>>;