using HelpDesk.Data.Query;
using HelpDesk.Entity;
using System.Linq;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: количество доступных для выбора пользователем типы работ по ТО (чтобы потом определился исполнитель)
    /// </summary>
    public class CountAllowableObjectTypeQuery : IQuery<int, OrganizationObjectTypeWorker, Employee>
    {
        private readonly long userId;

        public CountAllowableObjectTypeQuery(long userId)
        {
            this.userId = userId;
        }

        public int Run(IQueryable<OrganizationObjectTypeWorker> organizationObjectTypeWorkers,
            IQueryable<Employee> employees)
        {

            return (from ootw in organizationObjectTypeWorkers
                    join e in employees on ootw.Organization.Id equals e.Organization.Id
                    where e.Id == userId 
                        && ootw.ObjectType.Soft == false
                        && ootw.ObjectType.Archive == false
                    select ootw).Count();

        }
    }
}
