using HelpDesk.Data.Query;
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
        public RequestLastEventQuery(IEnumerable<long> requestIds)
        {
            this.requestIds = requestIds;
        }
                
        public IEnumerable<RequestEventDTO> Run(IQueryable<RequestEvent> events)
        {

            IEnumerable<long> ids = (from z in events
                                     where requestIds.Contains(z.RequestId) && z.StatusRequest.Id != (long)RawStatusRequestEnum.DateEnd
                                     group z by z.RequestId into g
                                     select g.Max(d => d.Id)).ToList();

            var q = from e in events
                    where ids.Contains(e.Id)
                    select new RequestEventDTO
                    {
                        RequestId = e.RequestId,
                        DateEvent = e.DateEvent,
                        Note = e.Note,
                        Transfer = e.StatusRequest.Id == (long)RawStatusRequestEnum.ExtendedDeadLine,
                        OrdGroup = e.OrdGroup
                    };

            return q.ToList();
        }
    }
}
