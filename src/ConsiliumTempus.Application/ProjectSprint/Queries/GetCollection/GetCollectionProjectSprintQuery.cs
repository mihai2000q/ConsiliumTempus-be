using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;

public sealed record GetCollectionProjectSprintQuery(
    Guid ProjectId,
    List<string> Search,
    bool FromThisYear)
    : IRequest<ErrorOr<GetCollectionProjectSprintResult>>;