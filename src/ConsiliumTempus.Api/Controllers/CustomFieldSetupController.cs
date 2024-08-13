using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

public sealed class CustomFieldSetupController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HttpGet("Project/{projectId:guid}")]
    public async Task<IActionResult> GetCollectionFromProject(
        GetCollectionCustomFieldSetupFromProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<GetCollectionCustomFieldSetupQuery>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            getCollectionResult =>
                Ok(Mapper.Map<GetCollectionCustomFieldSetupFromProjectResponse>(getCollectionResult)),
            Problem
        );
    }

    [HttpPost("Project")]
    public async Task<IActionResult> CreateOnProject(CreateCustomFieldSetupOnProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateCustomFieldSetupCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            createResult => Ok(Mapper.Map<CreateCustomFieldSetupOnProjectResponse>(createResult)),
            Problem
        );
    }
}