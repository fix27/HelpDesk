using HelpDesk.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HelpDesk.DataService.Interface
{
    public interface IAccessWorkerUserExpressionService
    {
        Expression<Func<BaseRequest, bool>> GetAccessPredicate(IEnumerable<AccessWorkerUser> list);
    }
}
