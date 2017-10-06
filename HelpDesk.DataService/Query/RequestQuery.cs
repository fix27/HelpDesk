using HelpDesk.Common;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Filters;
using HelpDesk.DataService.DTO;
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
    public class RequestQuery<T> : IQuery<IEnumerable<RequestDTO>, T>
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
            Expression<Func<BaseRequest, bool>> where = t => true;

            if (filter != null)
            {
                if (!String.IsNullOrWhiteSpace(filter.ObjectName))
                    where = where.AndAlso(t => t.Object.SoftName.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                        t.Object.HardType.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                        t.Object.Model.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                        t.Object.Model.Manufacturer.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                        t.Object.ObjectType.Name.ToUpper().Contains(filter.ObjectName.ToUpper()));

                where = where.AndAlso(t => (filter.DateInsert1 == null || t.DateInsert >= filter.DateInsert1) &&
                        (filter.DateInsert2 == null || t.DateInsert <= filter.DateInsert2) &&
                        (filter.DateEndPlan1 == null || t.DateEndPlan >= filter.DateEndPlan1) &&
                        (filter.DateEndPlan2 == null || t.DateEndPlan <= filter.DateEndPlan2));
                
                if (filter.Ids != null && filter.Ids.Any())
                    where = where.AndAlso(t => filter.Ids.Contains(t.Id));

                if (filter.RawStatusIds != null && filter.RawStatusIds.Any())
                    where = where.AndAlso(t => filter.RawStatusIds.Contains(t.Status.Id));

                if (!String.IsNullOrWhiteSpace(filter.DescriptionProblem))
                    where = where.AndAlso(t => t.DescriptionProblem.ToUpper().Contains(filter.DescriptionProblem.ToUpper()));

                if (!String.IsNullOrWhiteSpace(filter.WorkerName))
                    where = where.AndAlso(t => t.Worker.Name.ToUpper().Contains(filter.WorkerName.ToUpper()));

                if (!String.IsNullOrWhiteSpace(filter.EmployeeInfo))
                    where = where.AndAlso(t => t.Employee.FM.ToUpper().Contains(filter.EmployeeInfo.ToUpper()) ||
                    t.Employee.Phone.ToUpper() == filter.EmployeeInfo.ToUpper() ||
                    t.Employee.Cabinet.ToUpper() == filter.EmployeeInfo.ToUpper() ||
                    t.Employee.Organization.Name.ToUpper().Contains(filter.EmployeeInfo.ToUpper()) ||
                    t.Employee.Organization.Address.ToUpper().Contains(filter.EmployeeInfo.ToUpper()));

                if (filter.Archive)
                {
                    if (filter.ArchiveYear > 0)
                    {
                        if (filter.ArchiveMonth == 0)
                            where = where.AndAlso(t => t.DateInsert.Year == filter.ArchiveYear);
                        else
                            where = where.AndAlso(t => t.DateInsert.Year == filter.ArchiveYear && t.DateInsert.Month == filter.ArchiveMonth);
                    }
                }
            }
                            
            var filteredRequests = requests.Where(accessPredicate.AndAlso(where));
            
            if (pageInfo != null)
            {
                pageInfo.TotalCount = requests.Where(accessPredicate).Count();
                pageInfo.Count = filteredRequests.Count();

                if (pageInfo.PageSize > 0)
                    filteredRequests = filteredRequests
                        .Skip(pageInfo.PageSize * pageInfo.CurrentPage)
                        .Take(pageInfo.PageSize);
            }

            if (orderInfo != null)
            {
                switch (orderInfo.PropertyName)
                {
                    case "ObjectName":
                        if (orderInfo.Asc)
                            filteredRequests = filteredRequests.OrderBy(t => t.Object.ObjectType.Name)
                                .ThenBy(t => t.Object.SoftName)
                                .ThenBy(t => t.Object.Model.Name)
                                .ThenBy(t => t.Object.Model.Manufacturer.Name);
                        else
                            filteredRequests = filteredRequests.OrderByDescending(t => t.Object.ObjectType.Name)
                                .ThenByDescending(t => t.Object.SoftName)
                                .ThenByDescending(t => t.Object.Model.Name)
                                .ThenByDescending(t => t.Object.Model.Manufacturer.Name);
                        break;
                    case "EmployeeInfo":
                        if (orderInfo.Asc)
                            filteredRequests = filteredRequests.OrderBy(t => t.Employee.Organization.Name)
                                .ThenBy(t => t.Employee.Organization.Address)
                                .ThenBy(t => t.Employee.FM)
                                .ThenBy(t => t.Employee.IM)
                                .ThenBy(t => t.Employee.OT);
                        else
                            filteredRequests = filteredRequests.OrderByDescending(t => t.Employee.Organization.Name)
                                .ThenByDescending(t => t.Employee.Organization.Address)
                                .ThenByDescending(t => t.Employee.FM)
                                .ThenByDescending(t => t.Employee.IM)
                                .ThenByDescending(t => t.Employee.OT);
                        break;
                    case "Statuses":
                        if (orderInfo.Asc)
                            filteredRequests = filteredRequests.OrderBy(t => t.Status.Name);
                        else
                            filteredRequests = filteredRequests.OrderByDescending(t => t.Status.Name);
                        break;
                    default:
                        filteredRequests = filteredRequests.OrderBy(orderInfo.PropertyName, orderInfo.Asc);
                        break;
                }
            }

            return filteredRequests.Select(r =>
                new RequestDTO()
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
                    EmployeeOrganizationAddress = r.Employee.Organization.Address,
                    User = r.User
                }).ToList();
        }
    }
}
