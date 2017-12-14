using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.Entity;

namespace HelpDesk.ConsumerEventService.Helpers
{
    public static class RequestHelper
    {
        public static RequestDTO GetDTO(this BaseRequest r)
        {
            return new RequestDTO()
            {
                Id = r.Id,
                WorkerName = r.Worker.Name,
                Status = r.Status,
                Object = r.Object,
                DateEndFact = r.DateEndFact,
                DateEndPlan = r.DateEndPlan,
                DateInsert = r.DateInsert,
                DateUpdate = r.DateUpdate,
                DescriptionProblem = r.DescriptionProblem,
                CountCorrectionDateEndPlan = r.CountCorrectionDateEndPlan,
                EmployeeFM = r.Employee.FM,
                EmployeeIM = r.Employee.IM,
                EmployeeOT = r.Employee.OT,
                EmployeeCabinet = r.Employee.Cabinet,
                EmployeePhone = r.Employee.Phone,
                EmployeePostName = r.Employee.Post.Name,
                EmployeeOrganizationName = r.Employee.Organization.Name,
                EmployeeOrganizationAddress = r.Employee.Organization.Address,
                User = r.User
            };
        }
    }
}
