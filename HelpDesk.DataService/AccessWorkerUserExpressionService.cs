using HelpDesk.DataService.Interface;
using HelpDesk.Entity;
using HelpDesk.Common.Helpers;
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
        
        public Expression<Func<BaseRequest, bool>> GetAccessRequestPredicate(IEnumerable<AccessWorkerUser> list)
        {
            Expression<Func<BaseRequest, bool>> where = t => true;

            if (list == null || !list.Any())
                return where;

            IDictionary<TypeAccessWorkerUserEnum, IEnumerable<AccessWorkerUser>> dictIds = list
                .GroupBy(t => t.Type)
                .Select(t => new { Type = t.Key, AccessList = t.Where(d => d.Type == t.Key) })
                .ToDictionary(t => t.Type, t => t.AccessList);

            var objectIds       = dictIds.ContainsKey(TypeAccessWorkerUserEnum.Object)          ? dictIds[TypeAccessWorkerUserEnum.Object].Select(x => x.Object.Id): null;
            var objectTypeIds   = dictIds.ContainsKey(TypeAccessWorkerUserEnum.ObjectType)      ?  dictIds[TypeAccessWorkerUserEnum.ObjectType].Select(x => x.ObjectType.Id): null;
            var organizationIds = dictIds.ContainsKey(TypeAccessWorkerUserEnum.Organization)    ? dictIds[TypeAccessWorkerUserEnum.Organization].Select(x => x.Organization.Id) : null;
            var workerIds       = dictIds.ContainsKey(TypeAccessWorkerUserEnum.Worker)          ? dictIds[TypeAccessWorkerUserEnum.Worker].Select(x => x.Worker.Id) : null;
            var organizationAddresses = dictIds.ContainsKey(TypeAccessWorkerUserEnum.OrganizationAddress) ? dictIds[TypeAccessWorkerUserEnum.OrganizationAddress].Select(x => x.OrganizationAddress) : null;
            
            if(objectIds != null)
                where = where.AndAlso(t => objectIds.Contains(t.Object.Id));

            if (objectTypeIds != null)
                where = where.AndAlso(t => objectTypeIds.Contains(t.Object.ObjectType.Id));

            if (organizationIds != null)
                where = where.AndAlso(t => organizationIds.Contains(t.Employee.Organization.Id));

            if (workerIds != null)
                where = where.AndAlso(t => workerIds.Contains(t.Worker.Id));
                        
            if (organizationAddresses != null)
            {
                Expression<Func<BaseRequest, bool>> organizationAddressesExp = (t => true);
                foreach (string a in organizationAddresses)
                    organizationAddressesExp = organizationAddressesExp
                        .OrElse(t => t.Employee.Organization.Address.ToUpper().Contains(a.ToUpper()));

                where = where.AndAlso(organizationAddressesExp);
            }
           
            return where;
        }

        public Expression<Func<OrganizationObjectTypeWorker, bool>> GetAccessOrganizationObjectTypeWorkerPredicate(IEnumerable<AccessWorkerUser> list)
        {
            Expression<Func<OrganizationObjectTypeWorker, bool>> where = t => true;

            if (list == null || !list.Any())
                return where;

            IDictionary<TypeAccessWorkerUserEnum, IEnumerable<AccessWorkerUser>> dictIds = list
                .GroupBy(t => t.Type)
                .Select(t => new { Type = t.Key, AccessList = t.Where(d => d.Type == t.Key) })
                .ToDictionary(t => t.Type, t => t.AccessList);

            
            var objectTypeIds = dictIds.ContainsKey(TypeAccessWorkerUserEnum.ObjectType) ? dictIds[TypeAccessWorkerUserEnum.ObjectType].Select(x => x.ObjectType.Id) : null;
            var organizationIds = dictIds.ContainsKey(TypeAccessWorkerUserEnum.Organization) ? dictIds[TypeAccessWorkerUserEnum.Organization].Select(x => x.Organization.Id) : null;
            var workerIds = dictIds.ContainsKey(TypeAccessWorkerUserEnum.Worker) ? dictIds[TypeAccessWorkerUserEnum.Worker].Select(x => x.Worker.Id) : null;
            var organizationAddresses = dictIds.ContainsKey(TypeAccessWorkerUserEnum.OrganizationAddress) ? dictIds[TypeAccessWorkerUserEnum.OrganizationAddress].Select(x => x.OrganizationAddress) : null;

            
            if (objectTypeIds != null)
                where = where.AndAlso(t => objectTypeIds.Contains(t.ObjectType.Id));

            if (organizationIds != null)
                where = where.AndAlso(t => organizationIds.Contains(t.Organization.Id));

            if (workerIds != null)
                where = where.AndAlso(t => workerIds.Contains(t.Worker.Id));

            if (organizationAddresses != null)
            {
                Expression<Func<OrganizationObjectTypeWorker, bool>> organizationAddressesExp = (t => true);
                foreach (string a in organizationAddresses)
                    organizationAddressesExp = organizationAddressesExp
                        .OrElse(t => t.Organization.Address.ToUpper().Contains(a.ToUpper()));

                where = where.AndAlso(organizationAddressesExp);
            }

            return where;
        }

        public Expression<Func<RequestObject, bool>> GetAccessRequestObjectPredicate(IEnumerable<AccessWorkerUser> list)
        {
            Expression<Func<RequestObject, bool>> where = t => true;
            if (list == null || !list.Any())
                return where;

            IDictionary<TypeAccessWorkerUserEnum, IEnumerable<AccessWorkerUser>> dictIds = list
                .GroupBy(t => t.Type)
                .Select(t => new { Type = t.Key, AccessList = t.Where(d => d.Type == t.Key) })
                .ToDictionary(t => t.Type, t => t.AccessList);

            var objectIds = dictIds.ContainsKey(TypeAccessWorkerUserEnum.Object) ? dictIds[TypeAccessWorkerUserEnum.Object].Select(x => x.Object.Id) : null;
            var objectTypeIds = dictIds.ContainsKey(TypeAccessWorkerUserEnum.ObjectType) ? dictIds[TypeAccessWorkerUserEnum.ObjectType].Select(x => x.ObjectType.Id) : null;
            
            if (objectIds != null)
                where = where.AndAlso(t => objectIds.Contains(t.Id));

            if (objectTypeIds != null)
                where = where.AndAlso(t => objectTypeIds.Contains(t.ObjectType.Id));

            return where;
        }
    }
}
