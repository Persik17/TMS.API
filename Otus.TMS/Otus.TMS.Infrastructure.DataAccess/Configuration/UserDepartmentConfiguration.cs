using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.DataModels;

namespace Otus.TMS.Infrastructure.DataAccess.Configuration
{
    public class UserDepartmentConfiguration : IEntityTypeConfiguration<UserDepartment>
    {
        public void Configure(EntityTypeBuilder<UserDepartment> builder)
        {
            builder.ToTable("UserDepartments");
            builder.HasKey(ud => ud.Id);

            builder.HasOne(ud => ud.User)
                .WithMany()
                .HasForeignKey(ud => ud.UserId);

            builder.HasOne(ud => ud.Department)
                .WithMany()
                .HasForeignKey(ud => ud.DepartmentId);
        }
    }
}
