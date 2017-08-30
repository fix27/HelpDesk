using HelpDesk.DataService.Interface;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для получения accessPredicate
    /// </summary>
    public class AccessWorkerUserExpressionService : IAccessWorkerUserExpressionService
    {
        public Expression<Func<BaseRequest, bool>> GetAccessPredicate(IEnumerable<AccessWorkerUser> list)
        {
            return s => true;
        }
    }
}
