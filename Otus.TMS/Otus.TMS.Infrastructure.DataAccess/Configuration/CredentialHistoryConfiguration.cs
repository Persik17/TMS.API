using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.DataModels;

namespace Otus.TMS.Infrastructure.DataAccess.Configuration
{
    public class CredentialHistoryConfiguration : IEntityTypeConfiguration<CredentialHistory>
    {
        public void Configure(EntityTypeBuilder<CredentialHistory> builder)
        {
            builder.ToTable("CredentialHistorie");
            builder.HasKey(ch => ch.Id);

            builder.HasOne(ch => ch.Credential)
                .WithMany()
                .HasForeignKey(ch => ch.CredentialId);

            builder.HasOne(ch => ch.ChangedBy)
                .WithMany()
                .HasForeignKey(ch => ch.ChangedById);
        }
    }
}
