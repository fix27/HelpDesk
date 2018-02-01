using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Data.Query;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.DTO;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.ConsumerEventService.Query;
using HelpDesk.ConsumerEventService.Resources;
using HelpDesk.EventBus.Common.AppEvents.Interface;

namespace HelpDesk.ConsumerEventService.Handlers
{
    /// <summary>
    /// Обработчик события "Истекает срок по заявке"
    /// </summary>
    public class RequestDeedlineAppEventHandler : IAppEventHandler<IRequestDeedlineAppEvent>
    {
        private readonly ILog log;
        private readonly IQueryRunner queryRunner;
        private readonly IEnumerable<ISender> senders;
        public RequestDeedlineAppEventHandler(IQueryRunner queryRunner, ILog log,
            IEnumerable<ISender> senders)
        {
            this.queryRunner = queryRunner;
            this.log = log;
            this.senders = senders;
        }

        static object lockObj = new object();
        public async Task Handle(IRequestDeedlineAppEvent appEvent)
        {
            IEnumerable<UserDeedlineAppEventSubscribeDTO> list = null;
            lock (lockObj)
            {
                list = queryRunner.Run(new UserRequestDeedlineAppEventSubscribeQuery(appEvent.RequestIds));
            }
            log.InfoFormat("-----------------------------: list.Count = {0}", list.Count());
            if (list == null || !list.Any())
                return;
            foreach (var evnt in list)
            {
                evnt.BaseUrl = Program.BaseWorkerUrl;
                foreach (var sender in senders)
                {
                    await sender.SendAsync(evnt, Resource.Subject_RequestDeedlineAppEventConsumer, "RequestDeedlineAppEvent");
                    log.InfoFormat("RequestDeedlineAppEventHandler Send OK: Email = {0}", evnt.Email);
                }
            }
        }
    }
}