using HelpDesk.Entity;
using HelpDesk.DataService.DTO;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IWorkerUserService
    {
        WorkerUser Get(long id);
        WorkerUserDTO GetDTO(long id);
        WorkerUserDTO GetDTO(string userName);
        void Create(string email, string password);
        void SaveStartSessionFact(long userId, string ip);

        IEnumerable<RawStatusRequestDTO> GetListSubscribeStatus(long userId);
        void ChangeSubscribeRequestState(long userId, long requestStateId);
        void ChangeSubscribe(long userId);
    }
}
