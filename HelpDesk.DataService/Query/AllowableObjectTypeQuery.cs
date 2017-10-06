using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Common.Helpers;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: доступные для выбора пользователем типы работ по ТО (чтобы потом определился исполнитель)
    /// </summary>
    public class AllowableObjectTypeQuery : IQuery<IEnumerable<SimpleDTO>, OrganizationObjectTypeWorker, Employee>
    {
        private readonly long userId;

        public AllowableObjectTypeQuery(long userId)
        {
            this.userId = userId;
        }

        public IEnumerable<SimpleDTO> Run(IQueryable<OrganizationObjectTypeWorker> organizationObjectTypeWorkers,
            IQueryable<Employee> employees)
        {

            var q = from ootw in organizationObjectTypeWorkers
                    join e in employees on ootw.Organization.Id equals e.Organization.Id
                    where e.Id == userId 
                        && ootw.ObjectType.Soft == false
                        && ootw.ObjectType.Archive == false
                    orderby ootw.ObjectType.Name
                    select new SimpleDTO()
                    {
                        Id = ootw.ObjectType.Id,
                        Name = ootw.ObjectType.Name
                    };

            return q.ToList();
        }
    }
}
