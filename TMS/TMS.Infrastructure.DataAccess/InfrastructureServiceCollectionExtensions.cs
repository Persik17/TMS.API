using Microsoft.Extensions.DependencyInjection;
using TMS.Abstractions.Interfaces.Repositories;
using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Infrastructure.DataAccess.DataModels;
using TMS.Infrastructure.DataAccess.Repositories;

namespace TMS.Infrastructure.DataAccess
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuditableCommandRepository<Company>, CompanyRepository>();
            services.AddScoped<IAuditableQueryRepository<Company>, CompanyRepository>();
            services.AddScoped<IBoardRepository<Board>, BoardRepository>();
            services.AddScoped<IAuditableQueryRepository<Board>, BoardRepository>();
            services.AddScoped<IAuditableCommandRepository<BoardUser>, BoardUserRepository>();
            services.AddScoped<IAuditableQueryRepository<BoardUser>, BoardUserRepository>();
            services.AddScoped<IAuditableCommandRepository<BoardUserRole>, BoardUserRoleRepository>();
            services.AddScoped<IAuditableQueryRepository<BoardUserRole>, BoardUserRoleRepository>();
            services.AddScoped<IAuditableCommandRepository<Column>, ColumnRepository>();
            services.AddScoped<IAuditableQueryRepository<Column>, ColumnRepository>();
            services.AddScoped<IAuditableCommandRepository<Comment>, CommentRepository>();
            services.AddScoped<IAuditableQueryRepository<Comment>, CommentRepository>();
            services.AddScoped<IAuditableCommandRepository<Company>, CompanyRepository>();
            services.AddScoped<IAuditableQueryRepository<Company>, CompanyRepository>();
            services.AddScoped<IAuditableCommandRepository<CredentialHistory>, CredentialHistoryRepository>();
            services.AddScoped<IAuditableQueryRepository<CredentialHistory>, CredentialHistoryRepository>();
            services.AddScoped<ICredentialRepository<Credential>, CredentialRepository>();
            services.AddScoped<IAuditableCommandRepository<Department>, DepartmentRepository>();
            services.AddScoped<IAuditableQueryRepository<Department>, DepartmentRepository>();
            services.AddScoped<ICommandRepository<NotificationSetting>, NotificationSettingRepository>();
            services.AddScoped<IQueryRepository<NotificationSetting>, NotificationSettingRepository>();
            services.AddScoped<IAuditableCommandRepository<Permission>, PermissionRepository>();
            services.AddScoped<IAuditableQueryRepository<Permission>, PermissionRepository>();
            services.AddScoped<IAuditableCommandRepository<RolePermission>, RolePermissionRepository>();
            services.AddScoped<IAuditableQueryRepository<RolePermission>, RolePermissionRepository>();
            services.AddScoped<IRoleRepository<Role>, RoleRepository>();
            services.AddScoped<ITaskRepository<DataModels.Task>, TaskRepository>();
            services.AddScoped<IAuditableCommandRepository<TaskType>, TaskTypeRepository>();
            services.AddScoped<IAuditableQueryRepository<TaskType>, TaskTypeRepository>();
            services.AddScoped<IAuditableCommandRepository<TelegramAccount>, TelegramAccountRepository>();
            services.AddScoped<IAuditableQueryRepository<TelegramAccount>, TelegramAccountRepository>();
            services.AddScoped<IAuditableCommandRepository<UserDepartment>, UserDepartmentRepository>();
            services.AddScoped<IAuditableQueryRepository<UserDepartment>, UserDepartmentRepository>();
            services.AddScoped<ICommandRepository<RegistrationVerification>, RegistrationVerificationRepository>();
            services.AddScoped<IQueryRepository<RegistrationVerification>, RegistrationVerificationRepository>();
            services.AddScoped<IUserRepository<User>, UserRepository>();

            return services;
        }
    }
}