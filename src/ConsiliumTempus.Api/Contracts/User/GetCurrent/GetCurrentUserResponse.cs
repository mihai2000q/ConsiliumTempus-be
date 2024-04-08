using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.User.GetCurrent;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCurrentUserResponse(
    string FirstName,
    string LastName,
    string Email,
    string? Role,
    DateOnly? DateOfBirth);