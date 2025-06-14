using Microsoft.Extensions.DependencyInjection;
using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Abstractions.Interfaces.Security;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.DTOs.Board;
using TMS.Application.Models.DTOs.Authentication;
using TMS.Application.Models.DTOs.Column;
using TMS.Application.Models.DTOs.Company;
using TMS.Application.Models.DTOs.Department;
using TMS.Application.Models.DTOs.NotificationSetting;
using TMS.Application.Models.DTOs.Permission;
using TMS.Application.Models.DTOs.Registration;
using TMS.Application.Models.DTOs.Role;
using TMS.Application.Models.DTOs.Task;
using TMS.Application.Models.DTOs.TaskType;
using TMS.Application.Models.DTOs.User;
using TMS.Application.Services;
using TMS.Application.Utils;
using TMS.Infrastructure.DataAccess.DataModels;
using TMS.Infrastructure.DataAccess.Repositories;

namespace TMS.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
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
            services.AddScoped<IAuditableCommandRepository<TelegramAccount>, TelegramAccountRepository>();
            services.AddScoped<IAuditableQueryRepository<TelegramAccount>, TelegramAccountRepository>();

            services.AddScoped<IAuthenticationService<AuthenticationDto, AuthenticationResultDto>, AuthenticationService>();
            services.AddScoped<IRegistrationService<RegistrationDto, RegistrationResultDto>, RegistrationService>();
            services.AddScoped<IVerificationService<RegistrationConfirmationDto, ConfirmationResultDto>, VerificationService>();

            services.AddScoped<INotifyService, NotifyService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}