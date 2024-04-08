namespace ConsiliumTempus.Api.Contracts.User.Update;

public sealed record UpdateCurrentUserRequest(
    string FirstName,
    string LastName,
    string? Role,
    DateOnly? DateOfBirth);