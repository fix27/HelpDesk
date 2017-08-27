using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;


namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с системными настройками
    /// </summary>
    public class SettingsService : BaseService, ISettingsService
    {
        private readonly ISettingsRepository settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public Settings Get()
        {
            return settingsRepository.Get();
        }
    }
}
