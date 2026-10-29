using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.Common.ValueObjects;

public static class AuthorshipFactory
{
    public static void WithAuthorship<TEntity>(
        this TEntity entity,
        UserAggregate? author = null,
        DateTime? createdDateTime = null)
        where TEntity : IAuthored
    {
        var authorship = Authorship.Create(author ?? UserFactory.Create(), createdDateTime ?? DateTime.UtcNow);

        var property = entity.GetType().GetProperty(nameof(IAuthored.Authorship))
                       ?? throw new InvalidOperationException(
                           $"The type '{entity.GetType().Name}' does not have an authorship");
        property.SetValue(entity, authorship);
    }
}
