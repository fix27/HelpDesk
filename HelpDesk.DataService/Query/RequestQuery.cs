using HelpDesk.Common;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Filters;
using HelpDesk.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Common.Helpers;
using System;
using System.Linq.Expressions;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: заявки исполнителя/дипетчера
    /// </summary>
    public class RequestQuery<T> : IQuery<RequestDTO, T>
        where T : BaseRequest
    {
        private readonly OrderInfo orderInfo;
        private readonly PageInfo pageInfo;
        private readonly RequestFilter filter;
        private readonly Expression<Func<BaseRequest, bool>> accessPredicate;

        public RequestQuery(Expression<Func<BaseRequest, bool>> accessPredicate, RequestFilter filter, OrderInfo orderInfo, ref PageInfo pageInfo)
        {
            this.accessPredicate = accessPredicate;
            this.orderInfo = orderInfo;
            this.pageInfo = pageInfo;
            this.filter = filter;
        }

        public IEnumerable<RequestDTO> Run(IQueryable<T> requests)
        {

            var qb = (from r in requests.Where(accessPredicate)
                      select new RequestDTO()
                      {
                          Id = r.Id,
                          WorkerId = r.Worker.Id,
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
                          EmployeeOrganizationAddress = r.Employee.Organization.Address
                      });

            var q = qb;
            if (filter != null && !String.IsNullOrWhiteSpace(filter.ObjectName))
                q = q.Where(t => t.Object.SoftName.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                    t.Object.HardType.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                    t.Object.Model.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                    t.Object.Model.Manufacturer.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                    t.Object.ObjectType.Name.ToUpper().Contains(filter.ObjectName.ToUpper()));

            if (filter != null)
                q = q.Where(t => (filter.DateInsert1 == null || t.DateInsert >= filter.DateInsert1) &&
                    (filter.DateInsert2 == null || t.DateInsert <= filter.DateInsert2) &&
                    (filter.DateEndPlan1 == null || t.DateEndPlan >= filter.DateEndPlan1) &&
                    (filter.DateEndPlan2 == null || t.DateEndPlan <= filter.DateEndPlan2));

            if (filter != null && filter.Ids != null && filter.Ids.Any())
                q = q.Where(t => filter.Ids.Contains(t.Id));

            if (filter != null && filter.RawStatusIds != null && filter.RawStatusIds.Any())
            {
                q = q.Where(t => filter.RawStatusIds.Contains(t.Status.Id));
            }


            if (filter != null && !String.IsNullOrWhiteSpace(filter.DescriptionProblem))
                q = q.Where(t => t.DescriptionProblem.ToUpper().Contains(filter.DescriptionProblem.ToUpper()));

            if (filter != null && !String.IsNullOrWhiteSpace(filter.WorkerName))
                q = q.Where(t => t.WorkerName.ToUpper().Contains(filter.WorkerName.ToUpper()));

            if (filter != null && !String.IsNullOrWhiteSpace(filter.EmployeeInfo))
                q = q.Where(t => t.EmployeeFM.ToUpper().Contains(filter.EmployeeInfo.ToUpper()) ||
                t.EmployeePhone.ToUpper()==filter.EmployeeInfo.ToUpper() ||
                t.EmployeeCabinet.ToUpper() == filter.EmployeeInfo.ToUpper() ||
                t.EmployeeOrganizationName.ToUpper().Contains(filter.EmployeeInfo.ToUpper()) ||
                t.EmployeeOrganizationAddress.ToUpper().Contains(filter.EmployeeInfo.ToUpper()));

            if (filter.Archive)
            {
                if (filter.ArchiveYear > 0)
                {
                    if (filter.ArchiveMonth == 0)
                    {
                        q = q.Where(t => t.DateInsert.Year == filter.ArchiveYear);
                    }
                    else
                    {
                        q = q.Where(t => t.DateInsert.Year == filter.ArchiveYear && t.DateInsert.Month == filter.ArchiveMonth);
                    }
                }
                
            }

            if (orderInfo != null)
            {
                switch (orderInfo.PropertyName)
                {
                    case "ObjectName":
                        if(orderInfo.Asc)
                        q = q.OrderBy(t => t.Object.ObjectType.Name)
                            .ThenBy(t => t.Object.SoftName)
                            .ThenBy(t => t.Object.Model.Name)
                            .ThenBy(t => t.Object.Model.Manufacturer.Name);
                        else
                            q = q.OrderByDescending(t => t.Object.ObjectType.Name)
                            .ThenByDescending(t => t.Object.SoftName)
                            .ThenByDescending(t => t.Object.Model.Name)
                            .ThenByDescending(t => t.Object.Model.Manufacturer.Name);
                        break;
                    case "EmployeeInfo":
                        if (orderInfo.Asc)
                            q = q.OrderBy(t => t.EmployeeOrganizationName)
                                .ThenBy(t => t.EmployeeOrganizationAddress)
                                .ThenBy(t => t.EmployeeFM)
                                .ThenBy(t => t.EmployeeIM)
                                .ThenBy(t => t.EmployeeOT);
                        else
                            q = q.OrderByDescending(t => t.EmployeeOrganizationName)
                                .ThenByDescending(t => t.EmployeeOrganizationAddress)
                                .ThenByDescending(t => t.EmployeeFM)
                                .ThenByDescending(t => t.EmployeeIM)
                                .ThenByDescending(t => t.EmployeeOT);
                        break;
                    case "Statuses":
                        if (orderInfo.Asc)
                            q = q.OrderBy(t => t.Status.Name);
                        else
                            q = q.OrderByDescending(t => t.Status.Name);
                        break;
                    default:
                        q = q.OrderBy(orderInfo.PropertyName, orderInfo.Asc);
                        break;
                }
            }
                

            if (pageInfo != null)
            {
                pageInfo.TotalCount = qb.Count();
                pageInfo.Count = q.Count();
            }

            if (pageInfo != null && pageInfo.PageSize > 0)
                q = q.Skip(pageInfo.PageSize * pageInfo.CurrentPage).Take(pageInfo.PageSize);

            return q.ToList();
        }
    }
}
