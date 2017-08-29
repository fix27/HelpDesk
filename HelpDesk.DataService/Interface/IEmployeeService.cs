using HelpDesk.DTO;

namespace HelpDesk.DataService.Interface
{
    public interface IEmployeeService
    {
        EmployeeDTO Get(long id);
        void Save(EmployeeDTO dto);
        bool IsComplete(long id);
    }
}
