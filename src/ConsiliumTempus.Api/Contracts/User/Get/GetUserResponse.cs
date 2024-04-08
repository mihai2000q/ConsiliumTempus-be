using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.User.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetUserResponse(
    string FirstName,
    string LastName,
    string Email,
    string? Role);