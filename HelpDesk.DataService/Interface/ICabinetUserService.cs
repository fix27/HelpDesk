using HelpDesk.Entity;
using HelpDesk.DTO;

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
