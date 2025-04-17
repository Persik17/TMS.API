using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.DataModels;

namespace Otus.TMS.Infrastructure.DataAccess.Configuration
{
    public class TelegramAccountConfiguration : IEntityTypeConfiguration<TelegramAccount>
    {
        public void Configure(EntityTypeBuilder<TelegramAccount> builder)
        {
            builder.ToTable("TelegramAccounts");
            builder.HasKey(ta => ta.Id);

            builder.Property(ta => ta.NickName)
                .HasMaxLength(255);
        }
    }
}
