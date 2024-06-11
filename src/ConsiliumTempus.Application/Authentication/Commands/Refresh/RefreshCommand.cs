using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Commands.Refresh;

public sealed record RefreshCommand(
    string Token,
    Guid RefreshToken)
    : IRequest<ErrorOr<RefreshResult>>;