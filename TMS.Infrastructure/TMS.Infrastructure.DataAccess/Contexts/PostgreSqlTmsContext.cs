using Microsoft.EntityFrameworkCore;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.DataAccess.Contexts
{
    /// <summary>
    /// Represents the Entity Framework Core context for the TMS (Task Management System) application,
    /// configured to use a PostgreSQL database.
    /// </summary>
    public class PostgreSqlTmsContext : DbContext
    {
        /// <summary>
        /// Gets or sets the DbSet for Company entities.
        /// </summary>
        public DbSet<Company> Companies { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Board entities.
        /// </summary>
        public DbSet<Board> Boards { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for User entities.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Column entities.
        /// </summary>
        public DbSet<Column> Columns { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Comment entities.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Credential entities.
        /// </summary>
        public DbSet<Credential> Credentials { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for CredentialHistory entities.
        /// </summary>
        public DbSet<CredentialHistory> CredentialHistories { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for NotificationSetting entities.
        /// </summary>
        public DbSet<NotificationSetting> NotificationSettings { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Role entities.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Task entities.
        /// </summary>
        public DbSet<DataModels.Task> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for TaskType entities.
        /// </summary>
        public DbSet<TaskType> TaskTypes { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for TelegramAccount entities.
        /// </summary>
        public DbSet<TelegramAccount> TelegramAccounts { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for UserVerification entities.
        /// </summary>
        public DbSet<UserVerification> UserVerifications { get; set; }

        public DbSet<UserInvitation> UserInvitations { get; set; }
        public DbSet<SystemSettings> SystemSettings { get; set; }
        public DbSet<TaskFile> TaskFiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlTmsContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public PostgreSqlTmsContext(DbContextOptions<PostgreSqlTmsContext> options) : base(options)
        {
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="DbSet{TEntity}"/> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Users)
                .WithOne(u => u.Company)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Board>()
                .HasOne(b => b.Head)
                .WithMany()
                .HasForeignKey(b => b.HeadId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Board>()
                .HasMany(b => b.Users)
                .WithMany(u => u.Boards);

            modelBuilder.Entity<Board>()
                .HasOne(b => b.Company)
                .WithMany(c => c.Boards)
                .HasForeignKey(b => b.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.SystemSettings)
                .WithOne(s => s.User)
                .HasForeignKey<SystemSettings>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.NotificationSettings)
                .WithOne(s => s.User)
                .HasForeignKey<NotificationSetting>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskFile>()
                .HasOne(tf => tf.Task)
                .WithMany(t => t.Files)
                .HasForeignKey(tf => tf.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}