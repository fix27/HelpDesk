using HelpDesk.Entity;
using HelpDesk.DataService.DTO;

namespace HelpDesk.DataService.Interface
{
    public interface ICabinetUserService
    {
        CabinetUser Get(long id);
        CabinetUserDTO GetDTO(long id);
        CabinetUserDTO GetDTO(string userName);
        void Create(string email, string password);
        
    }
}
