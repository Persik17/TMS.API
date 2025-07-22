namespace TMS.Application.Abstractions.Cache
{
    public static class CacheKeys
    {
        public static string Company(Guid companyId) => $"company:{companyId}";
        public static string Board(Guid boardId) => $"board:{boardId}";
        public static string BoardsByCompany(Guid departmentId) => $"board_company:{departmentId}";
        public static string Column(Guid columnId) => $"column:{columnId}";
        public static string ColumnsByBoard(Guid boardId) => $"column_board:{boardId}";
        public static string User(Guid userId) => $"user:{userId}";
        public static string TaskType(Guid taskTypeId) => $"taskType:{taskTypeId}";
        public static string NotificationSetting(Guid notificationSettingID) => $"notification_setting:{notificationSettingID}";
    }
}
