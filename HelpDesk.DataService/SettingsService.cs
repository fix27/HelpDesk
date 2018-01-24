using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Linq;
using HelpDesk.Common.Aspects;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с системными настройками
    /// </summary>
    [Cache]
    public class SettingsService : BaseService, ISettingsService
    {
        private readonly IBaseRepository<Settings> settingsRepository;

        public SettingsService(IBaseRepository<Settings> settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        [Cache(CacheKeyTemplate = "Settings")]
        public Settings Get()
        {
            return settingsRepository.GetList().FirstOrDefault();
        }
    }
}
