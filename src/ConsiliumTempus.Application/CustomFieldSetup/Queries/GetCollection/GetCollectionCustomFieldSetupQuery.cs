using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;

public sealed record GetCollectionCustomFieldSetupQuery(
    Guid? WorkspaceId,
    Guid? ProjectId)
    : IRequest<ErrorOr<GetCollectionCustomFieldSetupResult>>;