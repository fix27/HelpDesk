using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using NHibernate;
using System.Linq;

namespace HelpDesk.Data.NHibernate.Repository
{
    public class SettingsRepository: BaseRepository<Settings>, ISettingsRepository
    {

        public SettingsRepository(ISession session): 
            base(session)
        {

        }
        public Settings Get()
        {
            return GetList().FirstOrDefault();
        }
    }
    
}
