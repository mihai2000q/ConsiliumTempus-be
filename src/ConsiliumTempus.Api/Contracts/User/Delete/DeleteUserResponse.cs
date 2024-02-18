using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.User.Delete;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record DeleteUserResponse(string Message);