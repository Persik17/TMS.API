namespace Otus.TMS.ViewModels
{
    public class BoardViewModel { public string Name { get; set; } }
    public class BoardUserViewModel { public Guid BoardId { get; set; } public Guid UserId { get; set; } }
    public class BoardUserRoleViewModel { public Guid BoardUserId { get; set; } public Guid RoleId { get; set; } }
    public class ColumnViewModel { public Guid BoardId { get; set; } public string Name { get; set; } }
    public class CommentViewModel { public Guid TaskId { get; set; } public string Text { get; set; } }
    public class CompanyViewModel { public string Name { get; set; } }
    public class CredentialHistoryViewModel { public Guid CredentialId { get; set; } public string Value { get; set; } }
    public class CredentialViewModel { public Guid UserId { get; set; } public string PasswordHash { get; set; } }
    public class DepartmentViewModel { public Guid CompanyId { get; set; } public string Name { get; set; } }
    public class NotificationSettingViewModel { public bool EmailNotificationsEnabled { get; set; } public bool PushNotificationsEnabled { get; set; } public bool TelegramNotificationsEnabled { get; set; } }
    public class PermissionViewModel { public string Name { get; set; } }
    public class RolePermissionViewModel { public Guid RoleId { get; set; } public Guid PermissionId { get; set; } }
    public class RoleViewModel { public string Name { get; set; } }
    public class TaskViewModel { public Guid ColumnId { get; set; } public string Name { get; set; } }
    public class TaskTypeViewModel { public string Name { get; set; } }
    public class TelegramAccountViewModel { public Guid UserId { get; set; } public string ChatId { get; set; } }
    public class UserDepartmentViewModel { public Guid UserId { get; set; } public Guid DepartmentId { get; set; } }
    public class UserViewModel { public string FirstName { get; set; } public string LastName { get; set; } }

}
