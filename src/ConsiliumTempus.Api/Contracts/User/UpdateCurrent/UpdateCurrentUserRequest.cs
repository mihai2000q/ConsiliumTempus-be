namespace ConsiliumTempus.Api.Contracts.User.UpdateCurrent;

public sealed record UpdateCurrentUserRequest(
    string FirstName,
    string LastName,
    string? Role,
    DateOnly? DateOfBirth);