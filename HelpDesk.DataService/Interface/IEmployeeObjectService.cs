using HelpDesk.Common;
using HelpDesk.DataService.Filters;
using HelpDesk.DataService.DTO;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IEmployeeObjectService
    {
        IEnumerable<EmployeeObjectDTO> GetListEmployeeObject(long employeeId, EmployeeObjectFilter filter, OrderInfo orderInfo, ref PageInfo pageInfo);
        IEnumerable<EmployeeObjectDTO> GetListEmployeeObject(long employeeId, string objectName = null);
        IEnumerable<RequestObjectISDTO> GetListAllowableObjectIS(long employeeId, string name = null);
        IEnumerable<SimpleDTO> GetListAllowableObjectType(long employeeId);

        IEnumerable<RequestObjectISDTO> GetListAllowableObjectIS(long userId, long employeeId, string name = null);
        IEnumerable<SimpleDTO> GetListAllowableObjectType(long userId, long employeeId);
        void AddIS(long employeeId, RequestObjectISDTO dto);
        void AddTO(long employeeId, RequestObjectTODTO dto);
        void Delete(long employeeId, long id);
        bool AllowableForSendRequest(long employeeId);
        
    }
}

