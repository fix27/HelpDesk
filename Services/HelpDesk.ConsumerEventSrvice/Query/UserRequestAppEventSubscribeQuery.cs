using HelpDesk.Common.Helpers;
using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Common.DTO;
using HelpDesk.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.ConsumerEventService.Query
{
    /// <summary>
    /// Запрос: списки рассылки для подписанных на событие пользователей Исполнителя (первый элемент Tuple) 
    /// и пользователей личного кабинета (второй элемент Tuple)
    /// </summary>
    public class UserRequestAppEventSubscribeQuery : IQuery<Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, IEnumerable<UserRequestAppEventSubscribeDTO>>, 
        Request, RequestEvent, CabinetUserEventSubscribe, WorkerUserEventSubscribe, AccessWorkerUser>
    {
        private readonly long requestEventId;
        private readonly Func<long, StatusRequestEnum> getEquivalenceByElement;
        public UserRequestAppEventSubscribeQuery(long requestEventId, Func<long, StatusRequestEnum> getEquivalenceByElement)
        {
            this.requestEventId = requestEventId;
            this.getEquivalenceByElement = getEquivalenceByElement;
        }
                
        public Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, IEnumerable<UserRequestAppEventSubscribeDTO>> Run(IQueryable<Request> requests, IQueryable<RequestEvent> events, 
            IQueryable<CabinetUserEventSubscribe> cabinetUserEventSubscribes,
            IQueryable<WorkerUserEventSubscribe> workerUserEventSubscribes,
            IQueryable<AccessWorkerUser> accessWorkerUser)
        {
            RequestEvent evnt = events.FirstOrDefault(t => t.Id == requestEventId);
            if (evnt == null)
                return null;

            Request request = requests.First(t => t.Id == evnt.RequestId);
            IEnumerable<long> userIds = accessWorkerUser
                .Where(t => t.Worker.Id == request.Worker.Id && 
                    (t.User.UserType.TypeCode == TypeWorkerUserEnum.Worker || t.User.UserType.TypeCode == TypeWorkerUserEnum.WorkerAndDispatcher))
                .Select(t => t.User.Id).ToList();

            IEnumerable<UserRequestAppEventSubscribeDTO> cs =
                cabinetUserEventSubscribes.Where(t => t.User.Id == request.Employee.Id && t.User.Subscribe)
                .Select(t => new UserRequestAppEventSubscribeDTO
                {
                    RequestId = request.Id,
                    RequestStatusName = getEquivalenceByElement(request.Status.Id).GetDisplayName(),
                    Email = t.User.Email,
                    DateEndPlan = request.DateEndPlan
                })
                .ToList();

            IEnumerable<UserRequestAppEventSubscribeDTO> ws =
                workerUserEventSubscribes.Where(t => userIds.Contains(t.User.Id) && t.User.Id != request.User.Id && t.User.Subscribe)
                .Select(t => new UserRequestAppEventSubscribeDTO
                {
                    RequestId = request.Id,
                    RequestStatusName = request.Status.Name,
                    Email = t.User.Email,
                    DateEndPlan = request.DateEndPlan
                })
                .ToList();

            return new Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, 
                             IEnumerable<UserRequestAppEventSubscribeDTO>>(ws, cs);
        }
    }
}
