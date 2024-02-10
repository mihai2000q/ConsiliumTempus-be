namespace ConsiliumTempus.Api.Contracts.Authentication.Login;

public sealed record LoginRequest(
    string Email,
    string Password);