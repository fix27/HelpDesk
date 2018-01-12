using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: Статистика активных заявок по состояниям
    /// </summary>
    public class RequestStateCountQuery : IQuery<IEnumerable<RequestStateCountDTO>, Request>
    {
        private readonly Expression<Func<BaseRequest, bool>> accessPredicate;

        public RequestStateCountQuery(Expression<Func<BaseRequest, bool>> accessPredicate)
        {
            this.accessPredicate = accessPredicate;           
        }

        public IEnumerable<RequestStateCountDTO> Run(IQueryable<Request> requests)
        {
          
            var q = from r in requests.Where(accessPredicate)
                    group r by new { r.Status.Id, r.Status.Name, r.Status.BackColor } into g
                    select new RequestStateCountDTO
                    {
                        StatusId = g.Key.Id,
                        Count = g.Count(),
                        StatusName = g.Key.Name,
                        StatusBackColor = g.Key.BackColor
                    };

            return q.ToList();
            
        }
    }
}
