using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Authentication.Register;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record RegisterResponse(string Token);