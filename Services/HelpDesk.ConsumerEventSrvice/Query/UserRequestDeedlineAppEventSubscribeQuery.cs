using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.Data.Query;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.ConsumerEventService.Query
{
    /// <summary>
    /// Запрос: список рассылки для подписанных на оповещение об истечении срока пользователей Исполнителя
    /// </summary>
    public class UserRequestDeedlineAppEventSubscribeQuery : IQuery<IEnumerable<UserDeedlineAppEventSubscribeDTO>, Request, WorkerUserEventSubscribe, AccessWorkerUser>
    {
        private readonly IEnumerable<long> requestIds;
        public UserRequestDeedlineAppEventSubscribeQuery(IEnumerable<long> requestIds)
        {
            this.requestIds = requestIds;
        }

        public IEnumerable<UserDeedlineAppEventSubscribeDTO> Run(IQueryable<Request> requests,
            IQueryable<WorkerUserEventSubscribe> workerUserEventSubscribes,
            IQueryable<AccessWorkerUser> accessWorkerUsers)
        {
            IEnumerable<Request> deedlineRequests = requests.Where(t => requestIds.Contains(t.Id)).ToList();
            if (!deedlineRequests.Any())
                return null;

            var q = accessWorkerUsers.ToList()
                .Where(t => deedlineRequests.Select(r => r.Worker.Id).Contains(t.Worker.Id))
                .GroupBy(a => a.User.Email)
                .Select(g => new GroupedAccessWorkerUser
                {
                    Email = g.Key,
                    WorkerIds = g.Select(w => w.Worker.Id)
                });

            if (!q.Any())
                return null;

            IList<UserDeedlineAppEventSubscribeDTO> list = new List<UserDeedlineAppEventSubscribeDTO>();
            foreach (var t in q)
            {
                list.Add(new UserDeedlineAppEventSubscribeDTO
                {
                    Email = t.Email,
                    Items = deedlineRequests
                        .Where(r => t.WorkerIds.Contains(r.Worker.Id))
                        .Select(r => new DeedlineItem
                        {
                            RequestId = r.Id,
                            RequestStatusName = r.Status.Name,
                            DateEndPlan = r.DateEndPlan
                        })
                });
            }

            return list;
        }

        class GroupedAccessWorkerUser
        {
            public string Email { get; set; }
            public IEnumerable<long> WorkerIds { get; set; }
        }
    }
}
