using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Announcement.Commands.Create;

public sealed record CreateAnnouncementCommand(
    Guid ProjectId, 
    string Title, 
    string Description)
    : IRequest<ErrorOr<CreateAnnouncementResult>>;