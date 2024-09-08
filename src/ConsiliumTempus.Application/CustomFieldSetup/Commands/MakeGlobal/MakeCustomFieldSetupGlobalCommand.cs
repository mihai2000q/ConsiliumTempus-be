using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;

public sealed record MakeCustomFieldSetupGlobalCommand(Guid Id) 
    : IRequest<ErrorOr<MakeCustomFieldSetupGlobalResult>>;