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
    /// Запрос: доступные для выбора сотрудником-заявителем типы работ по ТО (чтобы потом определился исполнитель)
    /// </summary>
    public class AllowableObjectTypeQuery : IQuery<IEnumerable<SimpleDTO>, OrganizationObjectTypeWorker, Employee>
    {
        private readonly long employeeId;
        private readonly Expression<Func<OrganizationObjectTypeWorker, bool>> accessPredicate;
        
        public AllowableObjectTypeQuery(Expression<Func<OrganizationObjectTypeWorker, bool>> accessPredicate, long employeeId)
            : this(employeeId)
        {
            this.accessPredicate = accessPredicate;
        }

        public AllowableObjectTypeQuery(long employeeId)
        {
            this.employeeId = employeeId;
        }

        public IEnumerable<SimpleDTO> Run(IQueryable<OrganizationObjectTypeWorker> organizationObjectTypeWorkers,
            IQueryable<Employee> employees)
        {

            var q = from ootw in organizationObjectTypeWorkers
                    join e in employees on ootw.Organization.Id equals e.Organization.Id
                    where e.Id == employeeId
                        && ootw.ObjectType.Soft == false
                        && ootw.ObjectType.Archive == false
                    orderby ootw.ObjectType.Name
                    select ootw;

            if(accessPredicate !=null)
                q = q.Where(accessPredicate);
            
            return q.Select(ootw => new SimpleDTO()
            {
                Id = ootw.ObjectType.Id,
                Name = ootw.ObjectType.Name
            }).Distinct().ToList();
        }
    }
}
