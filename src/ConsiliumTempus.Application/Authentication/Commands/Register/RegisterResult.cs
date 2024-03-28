namespace ConsiliumTempus.Application.Authentication.Commands.Register;

public sealed record RegisterResult(
    string Token,
    string RefreshToken);