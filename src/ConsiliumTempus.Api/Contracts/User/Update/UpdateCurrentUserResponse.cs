using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.User.Update;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record UpdateCurrentUserResponse(string Message);