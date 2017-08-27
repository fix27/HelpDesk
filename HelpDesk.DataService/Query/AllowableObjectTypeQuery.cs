using HelpDesk.Data.Query;
using HelpDesk.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Common.Helpers;
using HelpDesk.DataService.Interface;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: доступные для выбора пользователем типы работ по ТО (чтобы потом определился исполнитель)
    /// </summary>
    public class AllowableObjectTypeQuery : IQuery<SimpleDTO, OrganizationObjectTypeWorker, PersonalProfile>
    {
        private readonly long userId;
        private readonly IConstantStatusRequestService constantService;

        public AllowableObjectTypeQuery(long userId, IConstantStatusRequestService constantService)
        {
            this.userId = userId;
            this.constantService = constantService;
        }

        public IEnumerable<SimpleDTO> Run(IQueryable<OrganizationObjectTypeWorker> organizationObjectTypeWorkers,
            IQueryable<PersonalProfile> employees)
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
