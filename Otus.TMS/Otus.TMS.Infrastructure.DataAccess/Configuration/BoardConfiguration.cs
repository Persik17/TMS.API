using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.DataModels;

namespace Otus.TMS.Infrastructure.DataAccess.Configuration
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.ToTable("Board");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(b => b.Department)
                .WithMany()
                .HasForeignKey(b => b.DepartmentId);

            builder.HasMany(b => b.BoardUsers)
                .WithOne(bu => bu.Board)
                .HasForeignKey(bu => bu.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
