using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetOverview;

public sealed record GetOverviewProjectQuery(
    Guid Id)
    : IRequest<ErrorOr<GetOverviewProjectResult>>;