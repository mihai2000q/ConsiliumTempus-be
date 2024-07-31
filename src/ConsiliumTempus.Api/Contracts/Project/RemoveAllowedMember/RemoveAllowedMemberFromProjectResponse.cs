using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.RemoveAllowedMember;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record RemoveAllowedMemberFromProjectResponse(string Message);