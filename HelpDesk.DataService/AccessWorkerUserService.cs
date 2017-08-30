using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с правами исполнителя/диспетчера
    /// </summary>
    public class AccessWorkerUserService : BaseService, IAccessWorkerUserService
    {
        
        private readonly IBaseRepository<AccessWorkerUser> accessWorkerUserRepository;
        
        public AccessWorkerUserService(IBaseRepository<AccessWorkerUser> accessWorkerUserRepository)
        {
            this.accessWorkerUserRepository = accessWorkerUserRepository;
        }
        
        public IEnumerable<AccessWorkerUser> GetList(long userId)
        {
            return accessWorkerUserRepository.GetList(a => a.User.Id == userId)
                .OrderBy(a => a.Type).ToList();
        }
               
    }
}
