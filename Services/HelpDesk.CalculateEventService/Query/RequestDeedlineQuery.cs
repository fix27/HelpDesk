using HelpDesk.Data.Query;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using System;

namespace HelpDesk.CalculateEventService.Query
{
    /// <summary>
    /// Запрос: список Id заявок, у которых истекает срок исполнения
    /// </summary>
    public class RequestDeedlineQuery : IQuery<IEnumerable<long>, Request>
    {
        public IEnumerable<long> Run(IQueryable<Request> requests)
        {
            var q = requests
                .Where(r => r.Object.ObjectType.DeadlineHour > 0 && !r.DateEndFact.HasValue &&
                    r.DateEndPlan <= DateTime.Now.AddHours(r.Object.ObjectType.DeadlineHour) && r.DateEndPlan > DateTime.Now)
                .Select(r => r.Id);
            
            return q.ToList();
        }
    }
}
