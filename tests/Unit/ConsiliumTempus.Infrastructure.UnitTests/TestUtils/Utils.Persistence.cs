using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Persistence
    {
        internal static ConsiliumTempusDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ConsiliumTempusDbContext>()
                .UseSqlServer("Server=localhost;Database=ConsiliumTempus;User Id=sa;Password=x;Encrypt=false")
                .Options;

            return new ConsiliumTempusDbContext(
                new PublishDomainEventsInterceptor(Substitute.For<IPublisher>()),
                new StampInterceptor(Substitute.For<IHttpContextAccessor>()),
                options);
        }

        // What the interceptors receive when the changes are about to be saved.
        internal static DbContextEventData CreateEventData(DbContext? dbContext)
        {
            return new DbContextEventData(null!, (_, _) => string.Empty, dbContext);
        }
    }
}
