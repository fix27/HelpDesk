using HelpDesk.Entity;
using HelpDesk.DataService.DTO;

namespace HelpDesk.DataService.Interface
{
    public interface IWorkerUserService
    {
        WorkerUser Get(long id);
        WorkerUserDTO GetDTO(long id);
        WorkerUserDTO GetDTO(string userName);
        void Create(string email, string password);
        
    }
}
