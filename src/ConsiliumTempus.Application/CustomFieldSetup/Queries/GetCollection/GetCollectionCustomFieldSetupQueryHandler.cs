using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;

public sealed class GetCollectionCustomFieldSetupQueryHandler(ICustomFieldSetupRepository customFieldSetupRepository)
    : IRequestHandler<GetCollectionCustomFieldSetupQuery, ErrorOr<GetCollectionCustomFieldSetupResult>>
{
    public async Task<ErrorOr<GetCollectionCustomFieldSetupResult>> Handle(GetCollectionCustomFieldSetupQuery query, 
        CancellationToken cancellationToken)
    {
        var customFieldSetups = await customFieldSetupRepository.GetList(
            query.WorkspaceId.IfNotNull(WorkspaceId.Create),
            query.ProjectId.IfNotNull(ProjectId.Create),
            cancellationToken);
        return new GetCollectionCustomFieldSetupResult(customFieldSetups);
    }
}