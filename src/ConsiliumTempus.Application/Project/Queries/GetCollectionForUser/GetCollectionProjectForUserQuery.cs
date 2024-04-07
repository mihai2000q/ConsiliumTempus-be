using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;

public sealed record GetCollectionProjectForUserQuery : IRequest<ErrorOr<GetCollectionProjectForUserResult>>;