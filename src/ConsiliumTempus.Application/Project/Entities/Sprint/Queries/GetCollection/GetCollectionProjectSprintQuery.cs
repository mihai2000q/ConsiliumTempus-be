using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;

public sealed record GetCollectionProjectSprintQuery(
    Guid ProjectId)
    : IRequest<ErrorOr<GetCollectionProjectSprintResult>>;