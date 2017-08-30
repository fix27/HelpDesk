using HelpDesk.Entity;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IAccessWorkerUserService
    {
        IEnumerable<AccessWorkerUser> GetList(long userId);
        
    }
}
