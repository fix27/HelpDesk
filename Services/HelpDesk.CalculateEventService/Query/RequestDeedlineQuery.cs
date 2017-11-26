using HelpDesk.Data.Query;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.CalculateEventService.Query
{
    /// <summary>
    /// Запрос: список Id заявок, у которых истекает срок исполнения
    /// </summary>
    public class RequestDeedlineQuery : IQuery<IEnumerable<long>, Request>
    {
        /*if (Object.ObjectType.DeadlineHour > 0 && DateEndPlan.HasValue && !DateEndFact.HasValue)
                    return DateEndPlan.Value <= DateTime.Now.AddHours(Object.ObjectType.DeadlineHour) && DateEndPlan.Value > DateTime.Now;*/

        public IEnumerable<long> Run(IQueryable<Request> requests)
        {
            var q = requests
                .Where(r => r.Id > 0)
                .Select(r => r.Id);
            
            return q.ToList();
        }
    }
}
