using TMS.Application.Dto.User;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Extensions
{
    public static class SystemSettingsMappingExtensions
    {
        public static SystemSettingsDto ToSystemSettingsDto(this SystemSettings settings)
        {
            if (settings == null) return null;
            return new SystemSettingsDto
            {
                Id = settings.Id,
                UserId = settings.UserId,
                Theme = (int)settings.Theme,
                BoardBackgroundUrl = settings.BoardBackgroundUrl,
                BoardBackgroundName = settings.BoardBackgroundName
            };
        }
    }
}
