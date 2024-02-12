namespace ConsiliumTempus.Api.Contracts.Authentication.Register;

public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string? Role,
    DateOnly? DateOfBirth);