using HelpDesk.ConsumerEventSrvice.DTO;
using HelpDesk.Data.Query;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.ConsumerEventSrvice.Query
{
    /// <summary>
    /// Запрос: список рассылки для подписанных на событие пользователей личного кабинета и Исполнителя
    /// </summary>
    public class UserEventSubscribeQuery : IQuery<IEnumerable<UserEventSubscribeDTO>, Request, RequestEvent, CabinetUserEventSubscribe, WorkerUserEventSubscribe, AccessWorkerUser>
    {
        private readonly long requestEventId;
        public UserEventSubscribeQuery(long requestEventId)
        {
            this.requestEventId = requestEventId;
        }
                
        public IEnumerable<UserEventSubscribeDTO> Run(IQueryable<Request> requests, IQueryable<RequestEvent> events, 
            IQueryable<CabinetUserEventSubscribe> cabinetUserEventSubscribes,
            IQueryable<WorkerUserEventSubscribe> workerUserEventSubscribes,
            IQueryable<AccessWorkerUser> accessWorkerUser)
        {
            RequestEvent evnt = events.First(t => t.Id == requestEventId);
            Request request = requests.First(t => t.Id == evnt.RequestId);
            IEnumerable<long> userIds = accessWorkerUser
                .Where(t => t.Worker.Id == request.Worker.Id)
                .Select(t => t.User.Id).ToList();

            IEnumerable<UserEventSubscribeDTO> cs =
                cabinetUserEventSubscribes.Where(t => t.User.Id == request.Employee.Id)
                .Select(t => new UserEventSubscribeDTO())
                .ToList();
            IEnumerable<UserEventSubscribeDTO> ws =
                workerUserEventSubscribes.Where(t => userIds.Contains(t.User.Id))
                .Select(t => new UserEventSubscribeDTO())
                .ToList();
            
            return cs.Union(ws);
        }
    }
}
