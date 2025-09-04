using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.User;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly ISystemSettingsRepository _systemSettingsRepository;

        public SystemSettingsService(ISystemSettingsRepository systemSettingsRepository)
        {
            _systemSettingsRepository = systemSettingsRepository;
        }

        public async Task<SystemSettingsDto?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var settings = await _systemSettingsRepository.GetByUserIdAsync(userId, cancellationToken);
            if (settings == null) return null;

            return MapToDto(settings);
        }

        public async Task<SystemSettingsDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var settings = await _systemSettingsRepository.GetByIdAsync(id, cancellationToken);
            if (settings == null || settings.UserId != userId)
                throw new NotFoundException(typeof(SystemSettings));

            return MapToDto(settings);
        }

        public async Task<SystemSettingsDto> CreateAsync(SystemSettingsDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = new SystemSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Theme = (int)createDto.Theme,
                BoardBackgroundUrl = createDto.BoardBackgroundUrl,
                BoardBackgroundName = createDto.BoardBackgroundName,
            };
            await _systemSettingsRepository.InsertAsync(entity, cancellationToken);
            return MapToDto(entity);
        }

        public async Task<SystemSettingsDto> UpdateAsync(SystemSettingsDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _systemSettingsRepository.GetByUserIdAsync(userId, cancellationToken);
            if (entity == null)
                throw new NotFoundException(typeof(SystemSettings));

            entity.Theme = (int)dto.Theme;
            entity.BoardBackgroundUrl = dto.BoardBackgroundUrl;
            entity.BoardBackgroundName = dto.BoardBackgroundName;

            await _systemSettingsRepository.UpdateAsync(entity, cancellationToken);
            return MapToDto(entity);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _systemSettingsRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null || entity.UserId != userId)
                throw new NotFoundException(typeof(SystemSettings));

            await _systemSettingsRepository.DeleteAsync(id, cancellationToken);
        }

        private SystemSettingsDto MapToDto(SystemSettings settings)
        {
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