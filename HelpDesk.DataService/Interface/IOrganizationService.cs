using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IOrganizationService
    {
        IEnumerable<Organization> GetList(string name = null);
        IEnumerable<OrganizationDTO> GetListByWorkerUser(long userId, string name);
        IEnumerable<OrganizationDTO> GetListByWorkerUser(long userId, long? parentId);
        IEnumerable<Organization> GetList(long? parentId);
    }
}
