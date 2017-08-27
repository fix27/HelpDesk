using HelpDesk.Entity;
using HelpDesk.DTO;

namespace HelpDesk.DataService.Interface
{
    public interface IUserService
    {
        User Get(long id);
        UserDTO GetDTO(long id);
        UserDTO GetDTO(string userName);
        void Create(string email, string password);
        
    }
}
