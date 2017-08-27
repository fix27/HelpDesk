using HelpDesk.Common;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Filters;
using HelpDesk.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Common.Helpers;
using System;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: заявки пользователя
    /// </summary>
    public class PersonalRequestQuery<T> : IQuery<RequestDTO, T> 
        where T : BaseRequest
    {
        private readonly long userId;
        private readonly OrderInfo orderInfo;
        private readonly PageInfo pageInfo;
        private readonly RequestFilter filter;

        public PersonalRequestQuery(long userId, RequestFilter filter, OrderInfo orderInfo, PageInfo pageInfo)
            :this(userId, filter)
        {
            this.orderInfo = orderInfo;
            this.pageInfo = pageInfo;
        }

        public PersonalRequestQuery(long userId, RequestFilter filter)
            : this(userId)
        {
            this.filter = filter;
        }

        public PersonalRequestQuery(long userId)
        {
            this.userId = userId;
        }

        public IEnumerable<RequestDTO> Run(IQueryable<T> requests)
        {
        
            var qb = (from r in requests
                      where r.Employee.Id == userId
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
                           CountCorrectionDateEndPlan = r.CountCorrectionDateEndPlan
                      });

            var q = qb;
            if (filter!= null && !String.IsNullOrWhiteSpace(filter.ObjectName))
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

            if (filter != null && filter.StatusIds != null && filter.StatusIds.Any())
            {
                IList<long> statusIds = new List<long>();
                foreach (var s in filter.StatusIds)
                {
                    IEnumerable<long> items = StatusRequestFactorization.GetElementsByEquivalence(s);
                    foreach (var statuId in items)
                        statusIds.Add(statuId);
                }

                q = q.Where(t => statusIds.Contains(t.Status.Id));
            }
                

            if (filter != null && !String.IsNullOrWhiteSpace(filter.DescriptionProblem))
                q = q.Where(t => t.DescriptionProblem.ToUpper().Contains(filter.DescriptionProblem.ToUpper()));

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
