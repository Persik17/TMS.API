using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.DataModels;

namespace Otus.TMS.Infrastructure.DataAccess.Configuration
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(b => b.Company)
                .WithMany()
                .HasForeignKey(b => b.CompanyId);

            builder.HasOne(b => b.Head)
                .WithMany()
                .HasForeignKey(b => b.HeadId);
        }
    }
}
