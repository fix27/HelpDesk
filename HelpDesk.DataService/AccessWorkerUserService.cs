using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;

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

        public Expression<Func<BaseRequest, bool>> GetAccessExpression(long userId)
        {
            IEnumerable<AccessWorkerUser> list = accessWorkerUserRepository.GetList(a => a.User.Id == userId)
                .OrderBy(a => a.Type).ToList();

            return s => true;
        }
    }
}
