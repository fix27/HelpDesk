using HelpDesk.Common.Cache;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;

namespace HelpDesk.Data
{
    /// <summary>
    /// Декоратор для репозитория SettingsRepository, обеспечивающий кэширование
    /// </summary>
    public class SettingsManager: ISettingsRepository
    {
        private readonly ISettingsRepository settingsRepository;
        private readonly ICache memoryCache;

        public SettingsManager(ISettingsRepository settingsRepository, ICache memoryCache)
        {
            this.settingsRepository = settingsRepository;
            this.memoryCache = memoryCache;
        }
        public Settings Get()
        {
            return memoryCache.AddOrGetExisting(Settings.CACHE_KEY_TEMPLATE, 
                () => { return settingsRepository.Get(); });
        }
    }
}
