using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.ValueObjects;

internal static class AuditExtensions
{
    extension<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        /// <summary>
        /// The audit is a part of its owner: its columns are in the table of the owner, so it goes away with it.
        /// </summary>
        public void OwnsAudit(Expression<Func<TEntity, Audit?>> audit)
        {
            builder.OwnsOne(audit, a =>
            {
                a.Property(x => x.CreatedDateTime)
                    .HasColumnName(nameof(Audit.CreatedDateTime));
                a.Property(x => x.UpdatedDateTime)
                    .HasColumnName(nameof(Audit.UpdatedDateTime));

                a.HasOne(x => x.CreatedBy)
                    .WithMany()
                    .HasForeignKey(nameof(Audit.CreatedBy).ToId());
                a.Navigation(x => x.CreatedBy).AutoInclude();
                a.Property(nameof(Audit.CreatedBy).ToId())
                    .HasColumnName(nameof(Audit.CreatedBy).ToId());

                a.HasOne(x => x.UpdatedBy)
                    .WithMany()
                    .HasForeignKey(nameof(Audit.UpdatedBy).ToId());
                a.Navigation(x => x.UpdatedBy).AutoInclude();
                a.Property(nameof(Audit.UpdatedBy).ToId())
                    .HasColumnName(nameof(Audit.UpdatedBy).ToId());
            });

            builder.Navigation(audit).IsRequired();
        }
    }
}