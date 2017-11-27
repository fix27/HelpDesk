﻿using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.Data.Query;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.ConsumerEventService.Query
{
    /// <summary>
    /// Запрос: список рассылки для подписанных на событие пользователей личного кабинета и Исполнителя
    /// </summary>
    public class UserRequestAppEventSubscribeQuery : IQuery<IEnumerable<UserRequestAppEventSubscribeDTO>, Request, RequestEvent, CabinetUserEventSubscribe, WorkerUserEventSubscribe, AccessWorkerUser>
    {
        private readonly long requestEventId;
        public UserRequestAppEventSubscribeQuery(long requestEventId)
        {
            this.requestEventId = requestEventId;
        }
                
        public IEnumerable<UserRequestAppEventSubscribeDTO> Run(IQueryable<Request> requests, IQueryable<RequestEvent> events, 
            IQueryable<CabinetUserEventSubscribe> cabinetUserEventSubscribes,
            IQueryable<WorkerUserEventSubscribe> workerUserEventSubscribes,
            IQueryable<AccessWorkerUser> accessWorkerUser)
        {
            RequestEvent evnt = events.FirstOrDefault(t => t.Id == requestEventId);
            if (evnt == null)
                return null;

            Request request = requests.First(t => t.Id == evnt.RequestId);
            IEnumerable<long> userIds = accessWorkerUser
                .Where(t => t.Worker.Id == request.Worker.Id && (t.Worker.Id == 1 || t.Worker.Id == 3))
                .Select(t => t.User.Id).ToList();

            IEnumerable<UserRequestAppEventSubscribeDTO> cs =
                cabinetUserEventSubscribes.Where(t => t.User.Id == request.Employee.Id && t.User.Subscribe)
                .Select(t => new UserRequestAppEventSubscribeDTO
                {
                    RequestId = request.Id,
                    RequestStatusName = request.Status.Name,
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
            
            return cs.Union(ws);
        }
    }
}
