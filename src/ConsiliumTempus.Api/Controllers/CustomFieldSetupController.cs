using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.AddToProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.RemoveFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.UpdateWorkspace;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

public sealed class CustomFieldSetupController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasProjectAuthorization(ProjectAuthorizationLevel.IsAllowed)]
    [HasPermission(Permissions.ReadCustomFieldSetup)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(GetCustomFieldSetupRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetCustomFieldSetupQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            setup => Ok(Mapper.Map<GetCustomFieldSetupResponse>(setup)),
            Problem
        );
    }

    [HasPermission(Permissions.ReadCollectionCustomFieldSetupFromWorkspace)]
    [HttpGet("Workspace/{workspaceId:guid}")]
    public async Task<IActionResult> GetCollectionFromWorkspace(
        GetCollectionCustomFieldSetupFromWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetCollectionCustomFieldSetupQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getCollectionResult =>
                Ok(Mapper.Map<GetCollectionCustomFieldSetupFromWorkspaceResponse>(getCollectionResult)),
            Problem
        );
    }

    [HasProjectAuthorization(ProjectAuthorizationLevel.IsAllowed)]
    [HasPermission(Permissions.ReadCollectionCustomFieldSetupFromProject)]
    [HttpGet("Project/{projectId:guid}")]
    public async Task<IActionResult> GetCollectionFromProject(
        GetCollectionCustomFieldSetupFromProjectRequest request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetCollectionCustomFieldSetupQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getCollectionResult =>
                Ok(Mapper.Map<GetCollectionCustomFieldSetupFromProjectResponse>(getCollectionResult)),
            Problem
        );
    }

    [HasPermission(Permissions.CreateCustomFieldSetupOnWorkspace)]
    [HttpPost("Workspace")]
    public async Task<IActionResult> CreateOnWorkspace(CreateCustomFieldSetupOnWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateCustomFieldSetupCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            createResult => Ok(Mapper.Map<CreateCustomFieldSetupOnWorkspaceResponse>(createResult)),
            Problem
        );
    }

    [HasProjectAuthorization(ProjectAuthorizationLevel.IsAllowed)]
    [HasPermission(Permissions.CreateCustomFieldSetupOnProject)]
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

    [HasPermission(Permissions.AddCustomFieldSetupToProject)]
    [HttpPost("Add-Project")]
    public async Task<IActionResult> AddToProject(AddCustomFieldSetupToProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<AddCustomFieldSetupToProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            addProjectResult => Ok(Mapper.Map<AddCustomFieldSetupToProjectResponse>(addProjectResult)),
            Problem
        );
    }

    [HasProjectAuthorization(ProjectAuthorizationLevel.IsAllowed)]
    [HasPermission(Permissions.UpdateCustomFieldSetup)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCustomFieldSetupRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateCustomFieldSetupCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<UpdateCustomFieldSetupResponse>(deleteResult)),
            Problem
        );
    }

    [HasProjectAuthorization(ProjectAuthorizationLevel.IsAllowed)]
    [HasPermission(Permissions.UpdateWorkspaceCustomFieldSetup)]
    [HttpPut("Workspace")]
    public async Task<IActionResult> UpdateWorkspace(UpdateWorkspaceCustomFieldSetupRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateWorkspaceCustomFieldSetupCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateWorkspaceResult => Ok(Mapper.Map<UpdateWorkspaceCustomFieldSetupResponse>(updateWorkspaceResult)),
            Problem
        );
    }

    [HasProjectAuthorization(ProjectAuthorizationLevel.IsAllowed)]
    [HasPermission(Permissions.DeleteCustomFieldSetup)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(DeleteCustomFieldSetupRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<DeleteCustomFieldSetupCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteCustomFieldSetupResponse>(deleteResult)),
            Problem
        );
    }

    [HasPermission(Permissions.RemoveCustomFieldSetupFromProject)]
    [HttpDelete("{id:guid}/Remove-Project/{projectId:guid}")]
    public async Task<IActionResult> RemoveFromProject(RemoveCustomFieldSetupFromProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<RemoveCustomFieldSetupFromProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            removeProjectResult => Ok(Mapper.Map<RemoveCustomFieldSetupFromProjectResponse>(removeProjectResult)),
            Problem
        );
    }
}