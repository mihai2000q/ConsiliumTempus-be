namespace ConsiliumTempus.Api.Contracts.User.Update;

public sealed record UpdateUserRequest(
    Guid Id,
    string FirstName,
    string LastName,
    string? Role,
    DateOnly? DateOfBirth);