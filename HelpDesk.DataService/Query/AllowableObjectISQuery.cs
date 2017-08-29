using HelpDesk.Data.Query;
using HelpDesk.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Common.Helpers;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: доступные для выбора пользователем ИС
    /// </summary>
    public class AllowableObjectISQuery : IQuery<RequestObjectISDTO, RequestObject, OrganizationObjectTypeWorker, Employee>
    {
        private readonly long userId;
        private readonly string name;
        

        public AllowableObjectISQuery(long userId, string name)
            :this(userId)
        {
            this.name = name;
        }

        public AllowableObjectISQuery(long userId)
        {
            this.userId = userId;
        }

        public IEnumerable<RequestObjectISDTO> Run(IQueryable<RequestObject> objects, 
            IQueryable<OrganizationObjectTypeWorker> organizationObjectTypeWorkers,
            IQueryable<Employee> employees)
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
