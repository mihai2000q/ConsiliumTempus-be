using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

public sealed class ProjectController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasPermission(Permissions.CreateProject)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var token = GetToken();
        var command = Mapper.From(request)
            .AddParameters(ProjectMappingConfig.Token, token)
            .AdaptToType<CreateProjectCommand>();
        var result = await Mediator.Send(command, cancellationToken);
        
        return result.Match(
            createResult => Ok(Mapper.Map<CreateProjectResponse>(createResult)),
            Problem
        );
    }
}