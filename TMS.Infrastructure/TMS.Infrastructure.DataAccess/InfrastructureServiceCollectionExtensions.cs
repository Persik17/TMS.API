using Microsoft.Extensions.DependencyInjection;
using TMS.Infrastructure.Abstractions.Cache;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataAccess.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.DataAccess
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to register infrastructure-related services.
    /// </summary>
    public static class InfrastructureServiceCollectionExtensions
    {
        /// <summary>
        /// Adds infrastructure repositories to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            //Репозитории с расширенным функционалом
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<IColumnRepository, ColumnRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICredentialRepository, CredentialRepository>();
            services.AddScoped<INotificationSettingRepository, NotificationSettingRepository>();
            services.AddScoped<ISystemSettingsRepository, SystemSettingsRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();
            services.AddScoped<ITelegramAccountRepository, TelegramAccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserVerificationRepository, UserVerificationRepository>();
            services.AddScoped<IUserInvitationRepository, UserInvitationRepository>();

            //Репозитории с базовыми методами
            services.AddScoped<IAuditableCommandRepository<CredentialHistory>, CredentialHistoryRepository>();
            services.AddScoped<IAuditableQueryRepository<CredentialHistory>, CredentialHistoryRepository>();

            services.AddScoped<IRedisCacheContext, RedisCacheContext>();

            return services;
        }
    }
}