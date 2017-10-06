using HelpDesk.Data.Query;
using HelpDesk.Entity;
using System.Linq;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: количество доступных для выбора пользователем ИС
    /// </summary>
    public class CountAllowableObjectISQuery : IQuery<int, RequestObject, OrganizationObjectTypeWorker, Employee>
    {
        private readonly long userId;
        
        public CountAllowableObjectISQuery(long userId)
        {
            this.userId = userId;
        }

        public int Run(IQueryable<RequestObject> objects, 
            IQueryable<OrganizationObjectTypeWorker> organizationObjectTypeWorkers,
            IQueryable<Employee> employees)
        {

            return (from o in objects
                    join ootw in organizationObjectTypeWorkers on o.ObjectType.Id equals ootw.ObjectType.Id
                    join e in employees on ootw.Organization.Id equals e.Organization.Id
                    where o.ObjectType.Soft == true 
                        && e.Id == userId 
                        && o.ObjectType.Archive == false 
                        && o.Archive == false
                    select o).Count();

        }
    }
}
