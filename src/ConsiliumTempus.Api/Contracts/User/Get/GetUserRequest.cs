using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.User.Get;

public sealed record GetUserRequest
{
    [FromRoute]
    public Guid Id { get; init; }
}