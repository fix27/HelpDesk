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
        private readonly long requestId;
        public UserRequestDeedlineAppEventSubscribeQuery(long requestId)
        {
            this.requestId = requestId;
        }
                
        public IEnumerable<UserDeedlineAppEventSubscribeDTO> Run(IQueryable<Request> requests, 
            IQueryable<WorkerUserEventSubscribe> workerUserEventSubscribes,
            IQueryable<AccessWorkerUser> accessWorkerUser)
        {
            Request request = requests.FirstOrDefault(t => t.Id == requestId);
            if(request == null)
                return null;

            return accessWorkerUser
                .Where(t => t.Worker.Id == request.Worker.Id)
                .Select(t => new UserDeedlineAppEventSubscribeDTO
                {
                    RequestId = requestId,
                    RequestStatusName = request.Status.Name,
                    Email = t.User.Email,
                    DateEndPlan = request.DateEndPlan
                })
                .ToList();
            
        }
    }
}
