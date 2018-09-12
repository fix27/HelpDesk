using HelpDesk.Common;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Filters;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Common.Helpers;
using System;
using NHibernate;

namespace HelpDesk.DataService.Query
{
	public class EmployeeObjectQueryParam
	{
		public long EmployeeId { get; set; }
		public OrderInfo OrderInfo { get; set; }
		public PageInfo PageInfo { get; set; }
		public EmployeeObjectFilter Filter { get; set; }
	}

	/// <summary>
	/// Запрос: объекты заявок пользователя (профиль заявителя)
	/// </summary>
	public class EmployeeObjectQuery : IQuery<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>>
    {
		private readonly ISession _session;

		public EmployeeObjectQuery(ISession session)
		{
			_session = session;
		}

		public IEnumerable<EmployeeObjectDTO> Get(EmployeeObjectQueryParam param)
        {
			if (param == null)
				throw new ArgumentNullException("param");

			if (param.EmployeeId <= 0)
				throw new ArgumentException("param.EmployeeId <= 0");

			var qb = (from uo in _session.Query<EmployeeObject>()
                      where uo.Employee.Id == param.EmployeeId && uo.Object.Archive == false
                      select new EmployeeObjectDTO()
                      {
                          ObjectId = uo.Object.Id,
                          Id = uo.Id,
                          SoftName = uo.Object.SoftName,
                          ObjectType = uo.Object.ObjectType,
                          Soft = uo.Object.ObjectType.Soft,
                          HardType = uo.Object.HardType,
                          Model = uo.Object.Model,
                          ObjectTypeName = uo.Object.ObjectType.Name
                      });

            var q = qb;
            if (param.Filter != null && !String.IsNullOrWhiteSpace(param.Filter.ObjectName))
                q = qb.Where(t => t.SoftName.ToUpper().Contains(param.Filter.ObjectName.ToUpper()) ||
                t.HardType.Name.ToUpper().Contains(param.Filter.ObjectName.ToUpper()) ||
                t.Model.Name.ToUpper().Contains(param.Filter.ObjectName.ToUpper()) ||
                t.Model.Manufacturer.Name.ToUpper().Contains(param.Filter.ObjectName.ToUpper()) ||
                t.ObjectType.Name.ToUpper().Contains(param.Filter.ObjectName.ToUpper()));
                    

            if (param.Filter != null && param.Filter.Wares !=null && param.Filter.Wares.Any())
                q = q.Where(t => param.Filter.Wares.Contains(t.Soft));

            if (param.OrderInfo != null)
            {
                switch (param.OrderInfo.PropertyName)
                {
                    case "Wares":
                        if (param.OrderInfo.Asc)
                            q = q.OrderBy(t => t.Soft)
                                .ThenBy(t => t.ObjectType.Name);
                        else
                            q = q.OrderByDescending(t => t.Soft)
                                .ThenByDescending(t => t.ObjectType.Name);
                        break;
                    case "ObjectName":
                        if (param.OrderInfo.Asc)
                            q = q.OrderBy(t => t.SoftName)
                                .ThenBy(t => t.Model.Manufacturer.Name)
                                .ThenBy(t => t.Model.Name)
                                .ThenBy(t => t.ObjectType.Name)
                                .ThenBy(t => t.Soft);
                        else
                            q = q.OrderByDescending(t => t.SoftName)
                                .ThenByDescending(t => t.Model.Manufacturer.Name)
                                .ThenByDescending(t => t.Model.Name)
                                .ThenByDescending(t => t.ObjectType.Name)
                                .ThenByDescending(t => t.Soft);
                        break;
                    
                }
            }
                

            if (param.PageInfo != null)
            {
				param.PageInfo.TotalCount = qb.Count();
				param.PageInfo.Count = q.Count();
            }

            if (param.PageInfo != null && param.PageInfo.PageSize > 0)
                q = q.Skip(param.PageInfo.PageSize * param.PageInfo.CurrentPage).Take(param.PageInfo.PageSize);

            return q.ToList();
        }
    }
}
