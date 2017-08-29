using HelpDesk.Entity;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IAccessWorkerUserService
    {
        IEnumerable<AccessWorkerUser> GetList(long userId);
        Expression<Func<BaseRequest, bool>> GetAccessExpression(long userId);

    }
}
