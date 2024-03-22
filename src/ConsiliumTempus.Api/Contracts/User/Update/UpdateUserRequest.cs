namespace ConsiliumTempus.Api.Contracts.User.Update;

public sealed record UpdateUserRequest(
    string FirstName,
    string LastName,
    string? Role,
    DateOnly? DateOfBirth);