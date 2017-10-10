using HelpDesk.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HelpDesk.DataService.Interface
{
    public interface IAccessWorkerUserExpressionService
    {
        Expression<Func<BaseRequest, bool>> GetAccessRequestPredicate(IEnumerable<AccessWorkerUser> list);
        Expression<Func<OrganizationObjectTypeWorker, bool>> GetAccessOrganizationObjectTypeWorkerPredicate(IEnumerable<AccessWorkerUser> list);
        Expression<Func<RequestObject, bool>> GetAccessRequestObjectPredicate(IEnumerable<AccessWorkerUser> list);
    }
}
