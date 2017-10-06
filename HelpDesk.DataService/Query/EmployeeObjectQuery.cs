using HelpDesk.Common;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Filters;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Common.Helpers;
using System;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: объекты заявок пользователя (профиль заявителя)
    /// </summary>
    public class EmployeeObjectQuery : IQuery<IEnumerable<EmployeeObjectDTO>, EmployeeObject>
    {
        private readonly long employeeId;
        private readonly OrderInfo orderInfo;
        private readonly PageInfo pageInfo;
        private readonly EmployeeObjectFilter filter;


        public EmployeeObjectQuery(long employeeId, EmployeeObjectFilter filter, OrderInfo orderInfo, ref PageInfo pageInfo)
            :this(employeeId, filter)
        {
            this.orderInfo = orderInfo;
            this.pageInfo = pageInfo;
        }

        public EmployeeObjectQuery(long employeeId, EmployeeObjectFilter filter)
            : this(employeeId)
        {
            this.filter = filter;
        }

        public EmployeeObjectQuery(long employeeId)
        {
            this.employeeId = employeeId;
        }

        public IEnumerable<EmployeeObjectDTO> Run(IQueryable<EmployeeObject> employeeObjects)
        {
            
            var qb = (from uo in employeeObjects
                      where uo.Employee.Id == employeeId && uo.Object.Archive == false
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
            if (filter!= null && !String.IsNullOrWhiteSpace(filter.ObjectName))
                q = qb.Where(t => t.SoftName.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                t.HardType.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                t.Model.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                t.Model.Manufacturer.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                t.ObjectType.Name.ToUpper().Contains(filter.ObjectName.ToUpper()));
                    

            if (filter != null && filter.Wares !=null && filter.Wares.Any())
                q = q.Where(t => filter.Wares.Contains(t.Soft));

            if (orderInfo != null)
            {
                switch (orderInfo.PropertyName)
                {
                    case "Wares":
                        if (orderInfo.Asc)
                            q = q.OrderBy(t => t.Soft)
                                .ThenBy(t => t.ObjectType.Name);
                        else
                            q = q.OrderByDescending(t => t.Soft)
                                .ThenByDescending(t => t.ObjectType.Name);
                        break;
                    case "ObjectName":
                        if (orderInfo.Asc)
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
