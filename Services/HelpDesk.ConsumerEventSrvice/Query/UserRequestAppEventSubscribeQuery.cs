using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.Helpers;
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
        Request, RequestEvent, RequestArch, RequestEventArch, CabinetUserEventSubscribe, WorkerUserEventSubscribe, AccessWorkerUser>
    {
        private readonly long requestEventId;
        private readonly Func<long, StatusRequestEnum> getEquivalenceByElement;
        private readonly bool archive;

        public UserRequestAppEventSubscribeQuery(long requestEventId, bool archive, Func<long, StatusRequestEnum> getEquivalenceByElement)
        {
            this.requestEventId = requestEventId;
            this.getEquivalenceByElement = getEquivalenceByElement;
            this.archive = archive;
        }
                
        public Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, IEnumerable<UserRequestAppEventSubscribeDTO>> Run(
            IQueryable<Request> requests, IQueryable<RequestEvent> events,
            IQueryable<RequestArch> requestArchs, IQueryable<RequestEventArch> eventArchs,
            IQueryable<CabinetUserEventSubscribe> cabinetUserEventSubscribes,
            IQueryable<WorkerUserEventSubscribe> workerUserEventSubscribes,
            IQueryable<AccessWorkerUser> accessWorkerUser)
        {
            BaseRequest r = null;
            BaseRequestEvent evnt = null;

            if (archive)
            {
                evnt = eventArchs.FirstOrDefault(t => t.Id == requestEventId);

                if (evnt == null)
                    return null;

                r = requestArchs.First(t => t.Id == evnt.RequestId);
            }
            else
            {
                evnt = events.FirstOrDefault(t => t.Id == requestEventId);
                if (evnt == null)
                    return null;

                r = requests.First(t => t.Id == evnt.RequestId);
            }

            RequestDTO request = r.GetDTO();


            IEnumerable<long> userIds = accessWorkerUser
                .Where(t => t.Worker.Id == r.Worker.Id && 
                    (t.User.UserType.TypeCode == TypeWorkerUserEnum.Worker || 
                     t.User.UserType.TypeCode == TypeWorkerUserEnum.WorkerAndDispatcher))
                .Select(t => t.User.Id).ToList();

            IEnumerable<UserRequestAppEventSubscribeDTO> ws =
                workerUserEventSubscribes.Where(t => userIds.Contains(t.User.Id) && 
                    t.User.Id != request.User.Id && 
                    t.User.Subscribe)
                .Select(t => new UserRequestAppEventSubscribeDTO
                {
                    Request = request,
                    Email = t.User.Email,
                    IsWorker = true
                })
                .ToList();

            IEnumerable<UserRequestAppEventSubscribeDTO> cs =
                cabinetUserEventSubscribes.Where(t => t.User.Id == r.Employee.Id && 
                    t.User.Subscribe)
                .Select(t => new UserRequestAppEventSubscribeDTO
                {
                    Request = request,
                    Email = t.User.Email
                })
                .ToList();

            return new Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, 
                             IEnumerable<UserRequestAppEventSubscribeDTO>>(ws, cs);
        }
    }
}
