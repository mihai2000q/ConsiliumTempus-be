using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.User.Commands.Delete;

public sealed record DeleteUserResult(UserAggregate User);