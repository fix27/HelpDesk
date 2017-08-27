using HelpDesk.Common;
using HelpDesk.DataService.Filters;
using HelpDesk.DTO;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IRequestService
    {
        CreateOrUpdateRequestDTO Get(long id = 0);
        CreateOrUpdateRequestDTO GetNewByRequestId(long requestId);
        CreateOrUpdateRequestDTO GetNewByObjectId(long objectId);

        void Delete(long id);

        IEnumerable<RequestDTO> GetList(long userId, RequestFilter filter, OrderInfo orderInfo, PageInfo pageInfo);
        long Save(CreateOrUpdateRequestDTO dto);

        IEnumerable<StatusRequestDTO> GetListStatus(bool archive);
        int GetCountRequiresConfirmation(long userId);

        IEnumerable<Year> GetListArchiveYear(long userId);

        IEnumerable<RequestEventDTO> GetListRequestEvent(long requestId);
    }
}
