using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Common.Helpers;
using System.Linq.Expressions;
using System;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: доступные для выбора сотрудником-заявителем ИС
    /// </summary>
    public class AllowableObjectISQuery : IQuery<IEnumerable<RequestObjectISDTO>, RequestObject, OrganizationObjectTypeWorker, Employee>
    {
        private readonly long employeeId;
        private readonly string name;
        private readonly Expression<Func<RequestObject, bool>> accessPredicate;

        
        public AllowableObjectISQuery(long employeeId, string name)
            : this(employeeId)
        {
            this.name = name;
        }
        public AllowableObjectISQuery(Expression<Func<RequestObject, bool>> accessPredicate, long employeeId, string name)
            :this(employeeId)
        {
            this.name = name;
            this.accessPredicate = accessPredicate;
        }
        
        public AllowableObjectISQuery(long employeeId)
        {
            this.employeeId = employeeId;
        }

        public IEnumerable<RequestObjectISDTO> Run(IQueryable<RequestObject> objects, 
            IQueryable<OrganizationObjectTypeWorker> organizationObjectTypeWorkers,
            IQueryable<Employee> employees)
        {

            var q = from o in objects
                    join ootw in organizationObjectTypeWorkers on o.ObjectType.Id equals ootw.ObjectType.Id
                    join e in employees on ootw.Organization.Id equals e.Organization.Id
                    where o.ObjectType.Soft == true 
                        && e.Id == employeeId
                        && o.ObjectType.Archive == false 
                        && o.Archive == false                       
                    orderby o.SoftName, o.ObjectType.Name
                    select o;

            if (name != null)
                q = q.Where(o => o.SoftName != null && o.SoftName.ToUpper().Contains(name.ToUpper()) ||
                        o.SoftName == null && o.ObjectType.Name.ToUpper().Contains(name.ToUpper()));

            q = q.Where(accessPredicate);

            return q.Select(o => new RequestObjectISDTO()
            {
                Id = o.Id,
                SoftName = o.SoftName,
                ObjectTypeName = o.ObjectType.Name
            }).Distinct().ToList();
        }
    }
}
