using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;

public sealed record DeleteCustomFieldSetupRequest
{
    [FromRoute] public Guid Id { get; init; }
}