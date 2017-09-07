using HelpDesk.DTO;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IEmployeeService
    {
        EmployeeDTO Get(long id);
        IEnumerable<EmployeeDTO> GetList(string name);
        void Save(EmployeeDTO dto);
        bool IsComplete(long id);
    }
}
