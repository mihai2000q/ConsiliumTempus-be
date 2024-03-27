namespace ConsiliumTempus.Application.Authentication.Commands.Login;

public sealed record LoginResult(
    string Token,
    string RefreshToken);