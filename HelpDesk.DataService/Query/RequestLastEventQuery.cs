using HelpDesk.Data.Query;
using HelpDesk.DataService.Interface;
using HelpDesk.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: последнee событиe заявок из списка Id-заявок
    /// </summary>
    public class RequestLastEventQuery : IQuery<RequestEventDTO, RequestEvent>
    {
        private readonly IEnumerable<long> requestIds;
        private readonly IConstantStatusRequestService constantService;
        public RequestLastEventQuery(IEnumerable<long> requestIds, IConstantStatusRequestService constantService)
        {
            this.requestIds = requestIds;
            this.constantService = constantService;
        }
                
        public IEnumerable<RequestEventDTO> Run(IQueryable<RequestEvent> events)
        {
       
            var q = from e in events
                    where requestIds.Contains(e.RequestId) && e.StatusRequest.Id != constantService.DateEndStatusRequest
                    group e by e.RequestId into g
                    select new RequestEventDTO
                    {
                        RequestId = g.Key,
                        DateEvent = g.Max(t => t.DateEvent),
                        Note = g.Max(t => t.Name),
                        Transfer = g.Max(t => t.TypeRequestEventId).HasValue
                    };

            return q.ToList();
        }
    }
}
