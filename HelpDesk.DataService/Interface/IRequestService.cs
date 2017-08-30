using HelpDesk.Common;
using HelpDesk.DataService.Filters;
using HelpDesk.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IRequestService
    {
        CreateOrUpdateRequestDTO Get(long id = 0);
        CreateOrUpdateRequestDTO GetNewByRequestId(long requestId);
        CreateOrUpdateRequestDTO GetNewByObjectId(long objectId);

        void Delete(long id);

        IEnumerable<RequestDTO> GetListByEmployee(long employeeId, RequestFilter filter, OrderInfo orderInfo, ref PageInfo pageInfo);

        IEnumerable<RequestDTO> GetList(long userId, RequestFilter filter, OrderInfo orderInfo, ref PageInfo pageInfo);

        long Save(CreateOrUpdateRequestDTO dto);

        IEnumerable<StatusRequestDTO> GetListStatus(bool archive);
        IEnumerable<StatusRequest> GetListRawStatus(bool archive);

        int GetCountRequiresConfirmation(long employeeId);

        IEnumerable<Year> GetListArchiveYear(long employeeId);

        IEnumerable<RequestEventDTO> GetListRequestEvent(long requestId);
    }
}
