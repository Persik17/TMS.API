using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Otus.TMS.Infrastructure.DataAccess.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<DataModels.Task>
    {
        public void Configure(EntityTypeBuilder<DataModels.Task> builder)
        {
            builder.ToTable("Tasks");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(t => t.Board)
                .WithMany()
                .HasForeignKey(t => t.BoardId);

            builder.HasOne(t => t.Creator)
                .WithMany()
                .HasForeignKey(t => t.CreatorId);

            builder.HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.AssigneeId);

            builder.HasOne(t => t.TaskType)
                .WithMany()
                .HasForeignKey(t => t.TaskTypeId);

            builder.HasOne(t => t.ParentTask)
               .WithMany()
               .HasForeignKey(t => t.ParentTaskId);

            builder.HasOne(t => t.Column)
                .WithMany()
                .HasForeignKey(t => t.ColumnId);
        }
    }
}
