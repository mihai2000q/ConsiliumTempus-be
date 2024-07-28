using ConsiliumTempus.Domain.Announcement;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IAnnouncementRepository
{
    Task Add(AnnouncementAggregate announcement, CancellationToken cancellationToken = default);
}