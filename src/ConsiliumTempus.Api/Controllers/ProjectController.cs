using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.AddStatus;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.Contracts.Project.GetStatuses;
using ConsiliumTempus.Api.Contracts.Project.RemoveStatus;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Api.Contracts.Project.UpdateFavorites;
using ConsiliumTempus.Api.Contracts.Project.UpdateOverview;
using ConsiliumTempus.Api.Contracts.Project.UpdateStatus;
using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Application.Project.Queries.GetStatuses;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

public sealed class ProjectController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasPermission(Permissions.ReadProject)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(GetProjectRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetProjectQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getResult => Ok(Mapper
                .From(getResult)
                .AddParameters(ProjectMappingConfig.CurrentUser, getResult.CurrentUser)
                .AdaptToType<GetProjectResponse>()),
            Problem
        );
    }

    [HasPermission(Permissions.ReadOverviewProject)]
    [HttpGet("Overview/{id:guid}")]
    public async Task<IActionResult> GetOverview(GetOverviewProjectRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetOverviewProjectQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            projectOverview => Ok(Mapper.Map<GetOverviewProjectResponse>(projectOverview)),
            Problem
        );
    }

    [HasPermission(Permissions.ReadCollectionProject)]
    [HttpGet]
    public async Task<IActionResult> GetCollection(GetCollectionProjectRequest request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetCollectionProjectQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getCollectionResult => Ok(Mapper
                .From(getCollectionResult)
                .AddParameters(ProjectMappingConfig.CurrentUser, getCollectionResult.CurrentUser)
                .AdaptToType<GetCollectionProjectResponse>()),
            Problem
        );
    }

    [HasPermission(Permissions.ReadStatusesFromProject)]
    [HttpGet("{id:guid}/Statuses")]
    public async Task<IActionResult> GetStatuses(GetStatusesFromProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<GetStatusesFromProjectQuery>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            getStatusesResult => Ok(Mapper.Map<GetStatusesFromProjectResponse>(getStatusesResult)),
            Problem
        );
    }

    [HasPermission(Permissions.CreateProject)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            createResult => Ok(Mapper.Map<CreateProjectResponse>(createResult)),
            Problem
        );
    }

    [HasPermission(Permissions.AddStatusToProject)]
    [HttpPost("Add-Status")]
    public async Task<IActionResult> AddStatus(AddStatusToProjectRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<AddStatusToProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            addStatusResult => Ok(Mapper.Map<AddStatusToProjectResponse>(addStatusResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateProject)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateResult => Ok(Mapper.Map<UpdateProjectResponse>(updateResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateFavoritesProject)]
    [HttpPut("Favorites")]
    public async Task<IActionResult> UpdateFavorites(UpdateFavoritesProjectRequest request, 
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateFavoritesProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateFavoritesResult => Ok(Mapper.Map<UpdateFavoritesProjectResponse>(updateFavoritesResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateOverviewProject)]
    [HttpPut("Overview")]
    public async Task<IActionResult> UpdateOverview(UpdateOverviewProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateOverviewProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateOverviewResult => Ok(Mapper.Map<UpdateOverviewProjectResponse>(updateOverviewResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateStatusFromProject)]
    [HttpPut("Update-Status")]
    public async Task<IActionResult> UpdateStatus(UpdateStatusFromProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateStatusFromProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateStatusResult => Ok(Mapper.Map<UpdateStatusFromProjectResponse>(updateStatusResult)),
            Problem
        );
    }

    [HasPermission(Permissions.DeleteProject)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<DeleteProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteProjectResponse>(deleteResult)),
            Problem
        );
    }

    [HasPermission(Permissions.RemoveStatusFromProject)]
    [HttpDelete("{id:guid}/Remove-Status/{statusId:guid}")]
    public async Task<IActionResult> RemoveStatus(RemoveStatusFromProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<RemoveStatusFromProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            removeStatusResult => Ok(Mapper.Map<RemoveStatusFromProjectResponse>(removeStatusResult)),
            Problem
        );
    }
}