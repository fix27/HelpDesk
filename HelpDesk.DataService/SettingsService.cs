using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Linq;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с системными настройками
    /// </summary>
    public class SettingsService : BaseService, ISettingsService
    {
        private readonly IBaseRepository<Settings> settingsRepository;

        public SettingsService(IBaseRepository<Settings> settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public Settings Get()
        {
            return settingsRepository.GetList().FirstOrDefault();
        }
    }
}
