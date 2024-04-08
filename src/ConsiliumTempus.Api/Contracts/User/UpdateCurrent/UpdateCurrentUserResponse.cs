using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.User.UpdateCurrent;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record UpdateCurrentUserResponse(string Message);