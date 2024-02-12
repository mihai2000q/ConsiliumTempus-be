using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Dto;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record UserDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string? Role,
    DateOnly? DateOfBirth);