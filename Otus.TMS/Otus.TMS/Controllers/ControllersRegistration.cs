using Microsoft.AspNetCore.Mvc;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;
using Otus.TMS.ViewModels;

namespace Otus.TMS.Controllers
{
    public static class ControllersRegistration
    {
        public static void RegisterControllers(this IEndpointRouteBuilder app)
        {
            app.MapGroup("/boards")
                .MapBoardController();

            app.MapGroup("/boardUsers")
                .MapBoardUserController();

            app.MapGroup("/boardUserRoles")
                .MapBoardUserRoleController();

            app.MapGroup("/columns")
                .MapColumnController();

            app.MapGroup("/comments")
                .MapCommentController();

            app.MapGroup("/companies")
                .MapCompanyController();

            app.MapGroup("/credentialHistories")
                .MapCredentialHistoryController();

            app.MapGroup("/credentials")
                .MapCredentialController();

            app.MapGroup("/departments")
                .MapDepartmentController();

            app.MapGroup("/notificationSettings")
                .MapNotificationSettingController();

            app.MapGroup("/permissions")
                .MapPermissionController();

            app.MapGroup("/rolePermissions")
                .MapRolePermissionController();

            app.MapGroup("/roles")
                .MapRoleController();

            app.MapGroup("/tasks")
                .MapTaskController();

            app.MapGroup("/taskTypes")
                .MapTaskTypeController();

            app.MapGroup("/telegramAccounts")
                .MapTelegramAccountController();

            app.MapGroup("/userDepartments")
                .MapUserDepartmentController();

            app.MapGroup("/users")
                .MapUserController();

        }

        public static RouteGroupBuilder MapBoardController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<Board> repository, ILogger<Board> logger) =>
            {
                logger.LogInformation("Getting all Boards");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<Board> repository, ILogger<Board> logger) =>
            {
                logger.LogInformation("Getting Boards by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] BoardViewModel model, IAuditableCommandRepository<Board> repository, ILogger<Board> logger) =>
            {
                logger.LogInformation("Adding Board");
                var item = new Board()
                {
                    Name = model.Name
                };
                await repository.InsertAsync(item);
                return Results.Created($"/Boards/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapBoardUserController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<BoardUser> repository, ILogger<BoardUser> logger) =>
            {
                logger.LogInformation("Getting all BoardUsers");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<BoardUser> repository, ILogger<BoardUser> logger) =>
            {
                logger.LogInformation("Getting BoardUsers by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] BoardUserViewModel model, IAuditableCommandRepository<BoardUser> repository, ILogger<BoardUser> logger) =>
            {
                logger.LogInformation("Adding BoardUser");
                var item = new BoardUser()
                {
                    BoardId = model.BoardId,
                    UserId = model.UserId
                };
                await repository.InsertAsync(item);
                return Results.Created($"/BoardUsers/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapBoardUserRoleController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<BoardUserRole> repository, ILogger<BoardUserRole> logger) =>
            {
                logger.LogInformation("Getting all BoardUserRoles");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<BoardUserRole> repository, ILogger<BoardUserRole> logger) =>
            {
                logger.LogInformation("Getting BoardUserRoles by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] BoardUserRoleViewModel model, IAuditableCommandRepository<BoardUserRole> repository, ILogger<BoardUserRole> logger) =>
            {
                logger.LogInformation("Adding BoardUserRole");
                var item = new BoardUserRole()
                {
                };
                await repository.InsertAsync(item);
                return Results.Created($"/BoardUserRoles/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapColumnController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<Column> repository, ILogger<Column> logger) =>
            {
                logger.LogInformation("Getting all Columns");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<Column> repository, ILogger<Column> logger) =>
            {
                logger.LogInformation("Getting Columns by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] ColumnViewModel model, IAuditableCommandRepository<Column> repository, ILogger<Column> logger) =>
            {
                logger.LogInformation("Adding Column");
                var item = new Column()
                {
                };
                await repository.InsertAsync(item);
                return Results.Created($"/Columns/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapCommentController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<Comment> repository, ILogger<Comment> logger) =>
            {
                logger.LogInformation("Getting all Comments");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<Comment> repository, ILogger<Comment> logger) =>
            {
                logger.LogInformation("Getting Comments by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] CommentViewModel model, IAuditableCommandRepository<Comment> repository, ILogger<Comment> logger) =>
            {
                logger.LogInformation("Adding Comment");
                var item = new Comment()
                {
                    TaskId = model.TaskId,
                    Text = model.Text
                };
                await repository.InsertAsync(item);
                return Results.Created($"/Comments/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapCompanyController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<Company> repository, ILogger<Company> logger) =>
            {
                logger.LogInformation("Getting all Companies");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<Company> repository, ILogger<Company> logger) =>
            {
                logger.LogInformation("Getting Companies by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] CompanyViewModel model, IAuditableCommandRepository<Company> repository, ILogger<Company> logger) =>
            {
                logger.LogInformation("Adding Company");
                var item = new Company()
                {
                    Name = model.Name
                };
                await repository.InsertAsync(item);
                return Results.Created($"/Companies/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapCredentialHistoryController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<CredentialHistory> repository, ILogger<CredentialHistory> logger) =>
            {
                logger.LogInformation("Getting all CredentialHistories");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<CredentialHistory> repository, ILogger<CredentialHistory> logger) =>
            {
                logger.LogInformation("Getting CredentialHistories by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] CredentialHistoryViewModel model, IAuditableCommandRepository<CredentialHistory> repository, ILogger<CredentialHistory> logger) =>
            {
                logger.LogInformation("Adding CredentialHistory");
                var item = new CredentialHistory()
                {
                };
                await repository.InsertAsync(item);
                return Results.Created($"/CredentialHistories/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapCredentialController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<Credential> repository, ILogger<Credential> logger) =>
            {
                logger.LogInformation("Getting all Credentials");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<Credential> repository, ILogger<Credential> logger) =>
            {
                logger.LogInformation("Getting Credentials by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] CredentialViewModel model, IAuditableCommandRepository<Credential> repository, ILogger<Credential> logger) =>
            {
                logger.LogInformation("Adding Credential");
                var item = new Credential()
                {
                    UserId = model.UserId,
                    PasswordHash = model.PasswordHash
                };
                await repository.InsertAsync(item);
                return Results.Created($"/Credentials/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapDepartmentController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<Department> repository, ILogger<Department> logger) =>
            {
                logger.LogInformation("Getting all Departments");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<Department> repository, ILogger<Department> logger) =>
            {
                logger.LogInformation("Getting Departments by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] DepartmentViewModel model, IAuditableCommandRepository<Department> repository, ILogger<Department> logger) =>
            {
                logger.LogInformation("Adding Department");
                var item = new Department()
                {
                    CompanyId = model.CompanyId,
                    Name = model.Name
                };
                await repository.InsertAsync(item);
                return Results.Created($"/Departments/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapNotificationSettingController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IQueryRepository<NotificationSetting> repository, ILogger<NotificationSetting> logger) =>
            {
                logger.LogInformation("Getting all NotificationSettings");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IQueryRepository<NotificationSetting> repository, ILogger<NotificationSetting> logger) =>
            {
                logger.LogInformation("Getting NotificationSettings by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] NotificationSettingViewModel model, ICommandRepository<NotificationSetting> repository, ILogger<NotificationSetting> logger) =>
            {
                logger.LogInformation("Adding NotificationSetting");
                var item = new NotificationSetting()
                {
                    EmailNotificationsEnabled = model.EmailNotificationsEnabled,
                    PushNotificationsEnabled = model.PushNotificationsEnabled,
                    TelegramNotificationsEnabled = model.TelegramNotificationsEnabled
                };
                await repository.InsertAsync(item);
                return Results.Created($"/NotificationSettings/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapPermissionController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<Permission> repository, ILogger<Permission> logger) =>
            {
                logger.LogInformation("Getting all Permissions");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<Permission> repository, ILogger<Permission> logger) =>
            {
                logger.LogInformation("Getting Permissions by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] PermissionViewModel model, IAuditableCommandRepository<Permission> repository, ILogger<Permission> logger) =>
            {
                logger.LogInformation("Adding Permission");
                var item = new Permission()
                {
                    Name = model.Name
                };
                await repository.InsertAsync(item);
                return Results.Created($"/Permissions/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapRolePermissionController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<RolePermission> repository, ILogger<RolePermission> logger) =>
            {
                logger.LogInformation("Getting all RolePermissions");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<RolePermission> repository, ILogger<RolePermission> logger) =>
            {
                logger.LogInformation("Getting RolePermissions by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] RolePermissionViewModel model, IAuditableCommandRepository<RolePermission> repository, ILogger<RolePermission> logger) =>
            {
                logger.LogInformation("Adding RolePermission");
                var item = new RolePermission()
                {
                    RoleId = model.RoleId,
                    PermissionId = model.PermissionId
                };
                await repository.InsertAsync(item);
                return Results.Created($"/RolePermissions/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapRoleController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<Role> repository, ILogger<Role> logger) =>
            {
                logger.LogInformation("Getting all Roles");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<Role> repository, ILogger<Role> logger) =>
            {
                logger.LogInformation("Getting Roles by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] RoleViewModel model, IAuditableCommandRepository<Role> repository, ILogger<Role> logger) =>
            {
                logger.LogInformation("Adding Role");
                var item = new Role()
                {
                    Name = model.Name
                };
                await repository.InsertAsync(item);
                return Results.Created($"/Roles/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapTaskController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<Infrastructure.DataAccess.DataModels.Task> repository, ILogger<Infrastructure.DataAccess.DataModels.Task> logger) =>
            {
                logger.LogInformation("Getting all Tasks");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<Infrastructure.DataAccess.DataModels.Task> repository, ILogger<Infrastructure.DataAccess.DataModels.Task> logger) =>
            {
                logger.LogInformation("Getting Tasks by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] TaskViewModel model, IAuditableCommandRepository<Infrastructure.DataAccess.DataModels.Task> repository, ILogger<Infrastructure.DataAccess.DataModels.Task> logger) =>
            {
                logger.LogInformation("Adding Task");
                var item = new Infrastructure.DataAccess.DataModels.Task()
                {
                };
                await repository.InsertAsync(item);
                return Results.Created($"/Tasks/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapTaskTypeController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<Infrastructure.DataAccess.DataModels.TaskType> repository, ILogger<Infrastructure.DataAccess.DataModels.TaskType> logger) =>
            {
                logger.LogInformation("Getting all TaskTypes");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<Infrastructure.DataAccess.DataModels.TaskType> repository, ILogger<Infrastructure.DataAccess.DataModels.TaskType> logger) =>
            {
                logger.LogInformation("Getting TaskTypes by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] TaskTypeViewModel model, IAuditableCommandRepository<Infrastructure.DataAccess.DataModels.TaskType> repository, ILogger<Infrastructure.DataAccess.DataModels.TaskType> logger) =>
            {
                logger.LogInformation("Adding TaskType");
                var item = new TaskType()
                {
                    Name = model.Name
                };
                await repository.InsertAsync(item);
                return Results.Created($"/TaskTypes/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapTelegramAccountController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<TelegramAccount> repository, ILogger<TelegramAccount> logger) =>
            {
                logger.LogInformation("Getting all TelegramAccounts");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<TelegramAccount> repository, ILogger<TelegramAccount> logger) =>
            {
                logger.LogInformation("Getting TelegramAccounts by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] TelegramAccountViewModel model, IAuditableCommandRepository<TelegramAccount> repository, ILogger<TelegramAccount> logger) =>
            {
                logger.LogInformation("Adding TelegramAccount");
                var item = new TelegramAccount()
                {
                };
                await repository.InsertAsync(item);
                return Results.Created($"/TelegramAccounts/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapUserDepartmentController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<UserDepartment> repository, ILogger<UserDepartment> logger) =>
            {
                logger.LogInformation("Getting all UserDepartments");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<UserDepartment> repository, ILogger<UserDepartment> logger) =>
            {
                logger.LogInformation("Getting UserDepartments by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] UserDepartmentViewModel model, IAuditableCommandRepository<UserDepartment> repository, ILogger<UserDepartment> logger) =>
            {
                logger.LogInformation("Adding UserDepartment");
                var item = new UserDepartment()
                {
                    UserId = model.UserId,
                    DepartmentId = model.DepartmentId
                };
                await repository.InsertAsync(item);
                return Results.Created($"/UserDepartments/{item.Id}", item);
            });
            return group;
        }

        public static RouteGroupBuilder MapUserController(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromBody] IAuditableQueryRepository<User> repository, ILogger<User> logger) =>
            {
                logger.LogInformation("Getting all Users");
                return await repository.GetAllAsync();
            });

            group.MapGet("/{id:guid}", async (Guid id, IAuditableQueryRepository<User> repository, ILogger<User> logger) =>
            {
                logger.LogInformation("Getting Users by id");
                return await repository.GetByIdAsync(id);
            });

            group.MapPost("/", async ([FromBody] UserViewModel model, IAuditableCommandRepository<User> repository, ILogger<User> logger) =>
            {
                logger.LogInformation("Adding User");
                var item = new User()
                {

                };
                await repository.InsertAsync(item);
                return Results.Created($"/Users/{item.Id}", item);
            });
            return group;
        }
    }
}