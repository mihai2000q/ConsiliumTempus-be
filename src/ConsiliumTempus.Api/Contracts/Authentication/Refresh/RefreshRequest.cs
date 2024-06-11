namespace ConsiliumTempus.Api.Contracts.Authentication.Refresh;

public sealed record RefreshRequest(
    string Token,
    Guid RefreshToken);