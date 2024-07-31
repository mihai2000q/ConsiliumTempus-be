using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.Project.Queries.GetAllowedMembers;

public sealed record GetAllowedMembersFromProjectResult(
    IReadOnlyList<UserAggregate> AllowedMembers);