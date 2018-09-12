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
using NHibernate;

namespace HelpDesk.DataService.Query
{
	public interface IRequestQuery<T> : IQuery<RequestQueryParam, IEnumerable<RequestDTO>>
		where T : BaseRequest
	{

	}

	public class RequestQueryParam
	{
		public OrderInfo OrderInfo { get; set; }
		public PageInfo PageInfo { get; set; }
		public RequestFilter Filter { get; set; }
		public Expression<Func<BaseRequest, bool>> AccessPredicate { get; set; }
	}

	/// <summary>
	/// Запрос: заявки пользователя/исполнителя/дипетчера
	/// </summary>
	public class RequestQuery<T> : IRequestQuery<T>
		where T : BaseRequest
    {
		private readonly ISession _session;

		public RequestQuery(ISession session)
		{
			_session = session;
		}

		public IEnumerable<RequestDTO> Get(RequestQueryParam param)
        {
			if (param == null)
				throw new ArgumentNullException("param");

			Expression<Func<BaseRequest, bool>> where = t => true;

            if (param.Filter != null)
            {
                if (!String.IsNullOrWhiteSpace(param.Filter.ObjectName))
                    where = where.AndAlso(t => t.Object.SoftName.ToUpper().Contains(param.Filter.ObjectName.ToUpper()) ||
                        t.Object.HardType.Name.ToUpper().Contains(param.Filter.ObjectName.ToUpper()) ||
                        t.Object.Model.Name.ToUpper().Contains(param.Filter.ObjectName.ToUpper()) ||
                        t.Object.Model.Manufacturer.Name.ToUpper().Contains(param.Filter.ObjectName.ToUpper()) ||
                        t.Object.ObjectType.Name.ToUpper().Contains(param.Filter.ObjectName.ToUpper()));

                where = where.AndAlso(t => (param.Filter.DateInsert.Value1 == null || t.DateInsert >= param.Filter.DateInsert.Value1) &&
                        (param.Filter.DateInsert.Value2 == null || t.DateInsert <= param.Filter.DateInsert.Value2) &&
                        (param.Filter.DateEndPlan.Value1 == null || t.DateEndPlan >= param.Filter.DateEndPlan.Value1) &&
                        (param.Filter.DateEndPlan.Value2 == null || t.DateEndPlan <= param.Filter.DateEndPlan.Value2));
                
                if (param.Filter.Ids != null && param.Filter.Ids.Any())
                    where = where.AndAlso(t => param.Filter.Ids.Contains(t.Id));

                if (param.Filter.RawStatusIds != null && param.Filter.RawStatusIds.Any())
                    where = where.AndAlso(t => param.Filter.RawStatusIds.Contains(t.Status.Id));

                if (!String.IsNullOrWhiteSpace(param.Filter.DescriptionProblem))
                    where = where.AndAlso(t => t.DescriptionProblem.ToUpper().Contains(param.Filter.DescriptionProblem.ToUpper()));

                if (!String.IsNullOrWhiteSpace(param.Filter.WorkerName))
                    where = where.AndAlso(t => t.Worker.Name.ToUpper().Contains(param.Filter.WorkerName.ToUpper()));

                if (!String.IsNullOrWhiteSpace(param.Filter.EmployeeInfo))
                    where = where.AndAlso(t => t.Employee.FM.ToUpper().Contains(param.Filter.EmployeeInfo.ToUpper()) ||
                    t.Employee.Phone.ToUpper() == param.Filter.EmployeeInfo.ToUpper() ||
                    t.Employee.Cabinet.ToUpper() == param.Filter.EmployeeInfo.ToUpper() ||
                    t.Employee.Organization.Name.ToUpper().Contains(param.Filter.EmployeeInfo.ToUpper()) ||
                    t.Employee.Organization.Address.ToUpper().Contains(param.Filter.EmployeeInfo.ToUpper()));

                if (param.Filter.Archive)
                {
                    if (param.Filter.ArchiveYear > 0)
                    {
                        if (param.Filter.ArchiveMonth == 0)
                            where = where.AndAlso(t => t.DateInsert.Year == param.Filter.ArchiveYear);
                        else
                            where = where.AndAlso(t => t.DateInsert.Year == param.Filter.ArchiveYear && t.DateInsert.Month == param.Filter.ArchiveMonth);
                    }
                }
            }
                            
            var filteredRequests = _session.Query<T>().Where(param.AccessPredicate.AndAlso(where));
            
            

            if (param.OrderInfo != null)
            {
                switch (param.OrderInfo.PropertyName)
                {
                    case "ObjectName":
                        if (param.OrderInfo.Asc)
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
                        if (param.OrderInfo.Asc)
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
                        if (param.OrderInfo.Asc)
                            filteredRequests = filteredRequests.OrderBy(t => t.Status.Name);
                        else
                            filteredRequests = filteredRequests.OrderByDescending(t => t.Status.Name);
                        break;
                    default:
                        filteredRequests = filteredRequests.OrderBy(param.OrderInfo.PropertyName, param.OrderInfo.Asc);
                        break;
                }
            }

            var q = filteredRequests.Select(r =>
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
                    User = r.User,
                    Version = r.Version
                });


            if (param.PageInfo != null)
            {
                param.PageInfo.TotalCount = _session.Query<T>().Where(param.AccessPredicate).Count();
                param.PageInfo.Count = filteredRequests.Count();

                if (param.PageInfo.PageSize > 0)
                    q = q
                        .Skip(param.PageInfo.PageSize * param.PageInfo.CurrentPage)
                        .Take(param.PageInfo.PageSize);
            }

            return q.ToList();
        }
    }
}
