using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;

public sealed record GetCustomFieldSetupRequest
{
    [FromRoute] public Guid Id { get; init; }
}