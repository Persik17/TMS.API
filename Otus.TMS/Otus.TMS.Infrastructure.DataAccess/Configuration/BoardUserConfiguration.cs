using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.DataModels;

namespace Otus.TMS.Infrastructure.DataAccess.Configuration
{
    public class BoardUserConfiguration : IEntityTypeConfiguration<BoardUser>
    {
        public void Configure(EntityTypeBuilder<BoardUser> builder)
        {
            builder.ToTable("BoardUser");

            builder.HasKey(bc => new { bc.BoardId, bc.UserId });

            builder.HasOne(bc => bc.Board)
                .WithMany(b => b.BoardUsers)
                .HasForeignKey(bc => bc.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bc => bc.User)
                .WithMany(c => c.BoardUsers)
                .HasForeignKey(bc => bc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
