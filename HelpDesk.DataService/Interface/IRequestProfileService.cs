using HelpDesk.Common;
using HelpDesk.DataService.Filters;
using HelpDesk.DTO;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IRequestProfileService
    {
        IEnumerable<PersonalProfileObjectDTO> GetListPersonalObject(long userId, PersonalObjectFilter filter, OrderInfo orderInfo, PageInfo pageInfo);
        IEnumerable<PersonalProfileObjectDTO> GetListPersonalObject(long userId, string objectName = null);
        IEnumerable<RequestObjectISDTO> GetListAllowableObjectIS(long userId, string name = null);
        IEnumerable<SimpleDTO> GetListAllowableObjectType(long userId);
        void AddIS(long userId, RequestObjectISDTO dto);
        void AddTO(long userId, RequestObjectTODTO dto);
        void Delete(long userId, long id);
        bool AllowableForSendRequest(long userId);
    }
}
