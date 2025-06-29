using Microsoft.Extensions.DependencyInjection;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Factories;
using TMS.Application.Abstractions.Messaging;
using TMS.Application.Abstractions.Security;
using TMS.Application.Abstractions.Services;
using TMS.Application.Cache;
using TMS.Application.Factories;
using TMS.Application.Messaging;
using TMS.Application.Security;
using TMS.Application.Services;

namespace TMS.Application
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to register application-related services.
    /// </summary>
    public static class ApplicationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds application services to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IColumnService, ColumnService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<INotificationSettingService, NotificationSettingService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskTypeService, TaskTypeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccessService, AccessService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IVerificationService, VerificationService>();

            services.AddScoped<INotifyService, NotifyService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IUserFactory, UserFactory>();

            services.AddScoped<ITokenService, JwtTokenService>();

            services.AddScoped<ICacheService, CacheService>();

            return services;
        }
    }
}