using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.DataModels;

namespace Otus.TMS.Infrastructure.DataAccess.Configuration
{
    public class TaskTypeConfiguration : IEntityTypeConfiguration<TaskType>
    {
        public void Configure(EntityTypeBuilder<TaskType> builder)
        {
            builder.ToTable("TaskTypes");
            builder.HasKey(tt => tt.Id);

            builder.Property(tt => tt.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
