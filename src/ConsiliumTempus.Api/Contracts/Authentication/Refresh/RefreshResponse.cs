using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Authentication.Refresh;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record RefreshResponse(string Token);