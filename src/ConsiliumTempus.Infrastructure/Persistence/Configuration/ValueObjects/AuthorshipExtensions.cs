using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.ValueObjects;

internal static class AuthorshipExtensions
{
    extension<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        /// <summary>
        /// The authorship is a part of its owner: its columns are in the table of the owner, so it goes away with it.
        /// </summary>
        public void OwnsAuthorship(Expression<Func<TEntity, Authorship?>> authorship)
        {
            builder.OwnsOne(authorship, ConfigureAuthorship);

            builder.Navigation(authorship).IsRequired();
        }
    }

    extension<TOwner, TEntity>(OwnedNavigationBuilder<TOwner, TEntity> builder)
        where TOwner : class
        where TEntity : class
    {
        /// <summary>
        /// The same as for an entity, for the entities that are owned (i.e., the comments of a task).
        /// </summary>
        public void OwnsAuthorship(Expression<Func<TEntity, Authorship?>> authorship)
        {
            builder.OwnsOne(authorship, ConfigureAuthorship);

            builder.Navigation(authorship).IsRequired();
        }
    }

    private static void ConfigureAuthorship<TEntity>(OwnedNavigationBuilder<TEntity, Authorship> builder)
        where TEntity : class
    {
        builder.Property(x => x.CreatedDateTime)
            .HasColumnName(nameof(Authorship.CreatedDateTime));
        builder.Property(x => x.UpdatedDateTime)
            .HasColumnName(nameof(Authorship.UpdatedDateTime));

        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(nameof(Authorship.Author).ToId());
        builder.Navigation(x => x.Author).AutoInclude();
        builder.Property(nameof(Authorship.Author).ToId())
            .HasColumnName(nameof(Authorship.Author).ToId());
    }
}
