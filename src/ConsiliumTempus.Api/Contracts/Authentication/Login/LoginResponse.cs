using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Authentication.Login;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record LoginResponse(
    string Token);