using HelpDesk.DataService.DTO;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IEmployeeService
    {
        EmployeeDTO Get(long id);
        IEnumerable<EmployeeDTO> GetList(string name);
        IEnumerable<EmployeeDTO> GetListByWorkerUser(long userId, string name);
        IEnumerable<EmployeeDTO> GetListByOrganization(long organizationId);
        void Save(EmployeeDTO dto);
        bool IsComplete(long id);
    }
}
