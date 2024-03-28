using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Commands.Refresh;

public sealed record RefreshCommand(
    string Token, 
    string RefreshToken) 
    : IRequest<ErrorOr<RefreshResult>>;