using HelpDesk.Entity;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IOrganizationService
    {
        IEnumerable<Organization> GetList(string name = null);
        IEnumerable<Organization> GetList(long? parentId);
    }
}
