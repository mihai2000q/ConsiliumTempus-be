using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Announcement;
using ConsiliumTempus.Infrastructure.Persistence.Database;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class AnnouncementRepository(ConsiliumTempusDbContext dbContext) : IAnnouncementRepository
{
    public async Task Add(AnnouncementAggregate announcement, CancellationToken cancellationToken = default)
    {
        await dbContext.Announcements.AddAsync(announcement, cancellationToken);
    }
}