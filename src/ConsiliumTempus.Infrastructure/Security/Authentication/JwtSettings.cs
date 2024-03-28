﻿namespace ConsiliumTempus.Infrastructure.Security.Authentication;

public sealed class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string SecretKey { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; }
}