using HelpDesk.Entity;
using HelpDesk.DataService.DTO;
using System.Collections.Generic;
using HelpDesk.DataService.Common.DTO;

namespace HelpDesk.DataService.Interface
{
    public interface ICabinetUserService
    {
        CabinetUser Get(long id);
        CabinetUserDTO GetDTO(long id);
        CabinetUserDTO GetDTO(string userName);
        void Create(string email, string password);
        void SaveStartSessionFact(long userId, string ip);

        IEnumerable<StatusRequestDTO> GetListSubscribeStatus(long userId);
        void ChangeSubscribeRequestState(long userId, StatusRequestEnum requestState);
        void ChangeSubscribe(long userId);
    }
}
