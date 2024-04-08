using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.User.DeleteCurrent;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record DeleteCurrentUserResponse(string Message);