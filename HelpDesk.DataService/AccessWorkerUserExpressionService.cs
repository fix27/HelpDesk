using HelpDesk.DataService.Interface;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для получения accessPredicate. Все частные условия ч/з OR, общее - ч/з AND
    /// </summary>
    public class AccessWorkerUserExpressionService : IAccessWorkerUserExpressionService
    {
        public Expression<Func<BaseRequest, bool>> GetAccessPredicate(IEnumerable<AccessWorkerUser> list)
        {
            if (list == null || !list.Any())
                return _ => true;

            IDictionary<TypeAccessWorkerUserEnum, IEnumerable<AccessWorkerUser>> dictIds = list
                .Where(t => t.Type != TypeAccessWorkerUserEnum.OrganizationAddress)
                .GroupBy(t => t.Type)
                .Select(t => new { Type = t.Key, AccessList = t.Where(d => d.Type == t.Key) })
                .ToDictionary(t => t.Type, t => t.AccessList);

            var objectIds       = dictIds.ContainsKey(TypeAccessWorkerUserEnum.Object)      ? dictIds[TypeAccessWorkerUserEnum.Object].Select(x => x.Object.Id): Array.Empty<long>();
            var objectTypeIds   = dictIds.ContainsKey(TypeAccessWorkerUserEnum.ObjectType)  ?  dictIds[TypeAccessWorkerUserEnum.ObjectType].Select(x => x.ObjectType.Id): Array.Empty<long>(); 
            var organizationIds = dictIds.ContainsKey(TypeAccessWorkerUserEnum.Organization) ? dictIds[TypeAccessWorkerUserEnum.Organization].Select(x => x.Organization.Id) : Array.Empty<long>();
            var workerIds       = dictIds.ContainsKey(TypeAccessWorkerUserEnum.Worker)      ? dictIds[TypeAccessWorkerUserEnum.Worker].Select(x => x.Worker.Id) : Array.Empty<long>();

            return s => objectIds.Contains(s.Object.Id) &&
                objectTypeIds.Contains(s.Object.ObjectType.Id) &&
                organizationIds.Contains(s.Employee.Organization.Id) &&
                workerIds.Contains(s.Worker.Id); 

        }
    }
}
