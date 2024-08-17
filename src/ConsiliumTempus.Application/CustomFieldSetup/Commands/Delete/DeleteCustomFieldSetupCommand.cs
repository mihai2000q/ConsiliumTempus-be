using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;

public sealed record DeleteCustomFieldSetupCommand(Guid Id) : IRequest<ErrorOr<DeleteCustomFieldSetupResult>>;