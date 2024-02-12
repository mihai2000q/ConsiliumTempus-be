using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.User.Get;

public sealed class GetUserRequest
{
    [FromRoute]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public Guid Id { get; set; }
}