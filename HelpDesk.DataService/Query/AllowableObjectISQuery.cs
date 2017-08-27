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
    /// Запрос: доступные для выбора пользователем ИС
    /// </summary>
    public class AllowableObjectISQuery : IQuery<RequestObjectISDTO, RequestObject, OrganizationObjectTypeWorker, PersonalProfile>
    {
        private readonly long userId;
        private readonly string name;
        private readonly IConstantStatusRequestService constantService;


        public AllowableObjectISQuery(long userId, IConstantStatusRequestService constantService, string name):this(userId, constantService)
        {
            this.name = name;
        }

        public AllowableObjectISQuery(long userId, IConstantStatusRequestService constantService)
        {
            this.userId = userId;
            this.constantService = constantService;
        }

        public IEnumerable<RequestObjectISDTO> Run(IQueryable<RequestObject> objects, 
            IQueryable<OrganizationObjectTypeWorker> organizationObjectTypeWorkers,
            IQueryable<PersonalProfile> employees)
        {

            var q = from o in objects
                    join ootw in organizationObjectTypeWorkers on o.ObjectType.Id equals ootw.ObjectType.Id
                    join e in employees on ootw.Organization.Id equals e.Organization.Id
                    where o.ObjectType.Soft == true 
                        && e.Id == userId 
                        && o.ObjectType.Archive == false 
                        && o.Archive == false
                    orderby o.SoftName, o.ObjectType.Name
                    select o;

            if (name != null)
                q = q.Where(o => o.SoftName != null && o.SoftName.ToUpper().Contains(name.ToUpper()) ||
                        o.SoftName == null && o.ObjectType.Name.ToUpper().Contains(name.ToUpper()));

            return q.Select(o => new RequestObjectISDTO()
            {
                Id = o.Id,
                SoftName = o.SoftName,
                ObjectTypeName = o.ObjectType.Name
            }).ToList();
        }
    }
}
