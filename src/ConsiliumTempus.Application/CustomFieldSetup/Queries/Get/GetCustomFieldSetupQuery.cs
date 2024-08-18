using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;

public sealed record GetCustomFieldSetupQuery(Guid Id) : IRequest<ErrorOr<GetCustomFieldSetupResult>>;