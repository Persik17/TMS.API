using Microsoft.Extensions.DependencyInjection;
using TMS.Abstractions.Interfaces.Security;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.DTOs.Board;
using TMS.Application.DTOs.Column;
using TMS.Application.DTOs.Company;
using TMS.Application.DTOs.Department;
using TMS.Application.DTOs.NotificationSetting;
using TMS.Application.DTOs.Permission;
using TMS.Application.DTOs.Role;
using TMS.Application.DTOs.Task;
using TMS.Application.DTOs.TaskType;
using TMS.Application.DTOs.User;
using TMS.Application.Services;
using TMS.Application.Utils;

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
            services.AddScoped<IBoardService<BoardDto, BoardCreateDto>, BoardService>();
            services.AddScoped<IColumnService<ColumnDto, ColumnCreateDto>, ColumnService>();
            services.AddScoped<ICompanyService<CompanyDto, CompanyCreateDto>, CompanyService>();
            services.AddScoped<IDepartmentService<DepartmentDto, DepartmentCreateDto>, DepartmentService>();
            services.AddScoped<INotificationSettingService<NotificationSettingDto, NotificationSettingCreateDto>, NotificationSettingService>();
            services.AddScoped<IPermissionService<PermissionDto, PermissionCreateDto>, PermissionService>();
            services.AddScoped<IRoleService<RoleDto, RoleCreateDto>, RoleService>();
            services.AddScoped<ITaskService<TaskDto, TaskCreateDto>, TaskService>();
            services.AddScoped<ITaskTypeService<TaskTypeDto, TaskTypeCreateDto>, TaskTypeService>();
            services.AddScoped<IUserService<UserDto, UserCreateDto>, UserService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IVerificationService, VerificationService>();

            services.AddScoped<INotifyService, NotifyService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}