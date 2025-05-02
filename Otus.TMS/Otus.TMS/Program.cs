using Microsoft.EntityFrameworkCore;
using Otus.TMS.Controllers;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;
using Otus.TMS.Infrastructure.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Регистрация репозиториев
builder.Services.AddScoped<IAuditableCommandRepository<Board>, BoardRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<Board>, BoardRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<BoardUser>, BoardUserRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<BoardUser>, BoardUserRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<BoardUserRole>, BoardUserRoleRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<BoardUserRole>, BoardUserRoleRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<Column>, ColumnRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<Column>, ColumnRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<Comment>, CommentRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<Comment>, CommentRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<Company>, CompanyRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<Company>, CompanyRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<CredentialHistory>, CredentialHistoryRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<CredentialHistory>, CredentialHistoryRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<Credential>, CredentialRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<Credential>, CredentialRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<Department>, DepartmentRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<Department>, DepartmentRepository>();

builder.Services.AddScoped<ICommandRepository<NotificationSetting>, NotificationSettingRepository>();
builder.Services.AddScoped<IQueryRepository<NotificationSetting>, NotificationSettingRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<Permission>, PermissionRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<Permission>, PermissionRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<RolePermission>, RolePermissionRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<RolePermission>, RolePermissionRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<Role>, RoleRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<Role>, RoleRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<Otus.TMS.Infrastructure.DataAccess.DataModels.Task>, TaskRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<Otus.TMS.Infrastructure.DataAccess.DataModels.Task>, TaskRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<TaskType>, TaskTypeRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<TaskType>, TaskTypeRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<TelegramAccount>, TelegramAccountRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<TelegramAccount>, TelegramAccountRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<UserDepartment>, UserDepartmentRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<UserDepartment>, UserDepartmentRepository>();

builder.Services.AddScoped<IAuditableCommandRepository<User>, UserRepository>();
builder.Services.AddScoped<IAuditableQueryRepository<User>, UserRepository>();

# region Logger configuration

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
});
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

# endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PostgreSqlTmsContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=otusTMS;Username=postgres;Password=3353"));


var app = builder.Build();
app.RegisterControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
