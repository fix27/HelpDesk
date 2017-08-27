using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.DataService.Specification;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для запроса данных из справочника организаций
    /// </summary>
    public class OrganizationService : BaseService, IOrganizationService
    {
        
        private readonly IBaseRepository<Organization> organizationRepository;
        
        public OrganizationService(
            IBaseRepository<Organization> organizationRepository)
        {
            this.organizationRepository = organizationRepository;
        }
        
        public IEnumerable<Organization> GetList(string name = null)
        {
            if(string.IsNullOrWhiteSpace(name))
                return organizationRepository.GetList().OrderBy(p => p.Name).ToList();

            return organizationRepository.GetList(new SimpleEntityByNameLikeSpecification<Organization>(name))
                .OrderBy(p => p.Name).ToList();
        }

        public IEnumerable<Organization> GetList(long? parentId)
        {
            return organizationRepository.GetList(o => o.ParentId == parentId)
                .OrderBy(p => p.Name).ToList();
        }

    }
}
