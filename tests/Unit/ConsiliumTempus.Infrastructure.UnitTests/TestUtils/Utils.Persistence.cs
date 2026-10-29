using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
                new StampAuditInterceptor(Substitute.For<IHttpContextAccessor>()),
                options);
        }
    }
}
