using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;

public sealed record GetCollectionProjectSprintQuery(
    Guid ProjectId)
    : IRequest<ErrorOr<GetCollectionProjectSprintResult>>;