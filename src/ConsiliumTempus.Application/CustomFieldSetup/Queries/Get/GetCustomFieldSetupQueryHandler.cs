using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;

public sealed class GetCustomFieldSetupQueryHandler(ICustomFieldSetupRepository customFieldSetupRepository)
    : IRequestHandler<GetCustomFieldSetupQuery, ErrorOr<CustomFieldSetupAggregate>>
{
    public async Task<ErrorOr<CustomFieldSetupAggregate>> Handle(GetCustomFieldSetupQuery query,
        CancellationToken cancellationToken)
    {
        var setup = await customFieldSetupRepository.Get(CustomFieldSetupId.Create(query.Id), cancellationToken);
        return setup is not null ? setup : Errors.CustomFieldSetup.NotFound;
    }
}