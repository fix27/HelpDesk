using System;
using System.Threading.Tasks;
using MassTransit.Logging;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.ConsumerEventService.Query;
using HelpDesk.Data.Query;
using HelpDesk.ConsumerEventService.DTO;
using System.Collections.Generic;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.Resources;
using System.Linq;
using HelpDesk.DataService.Common.Interface;


namespace HelpDesk.ConsumerEventService.Handlers
{
    /// <summary>
    /// Обработчик события "Изменилось состояние заявки"
    /// </summary>
    public class RequestAppEventHandler : IAppEventHandler<IRequestAppEvent>
    {

        private readonly IQueryRunner queryRunner;
        private readonly IStatusRequestMapService statusRequestMapService;
        private readonly ILog log;
        private readonly IEnumerable<ISender> senders;
        public RequestAppEventHandler(IQueryRunner queryRunner, IStatusRequestMapService statusRequestMapService, ILog log, IEnumerable<ISender> senders)
        {
            this.queryRunner = queryRunner;
            this.statusRequestMapService = statusRequestMapService;
            this.log = log;
            this.senders = senders;
        }

        static object lockObj = new object();
        public async Task Handle(IRequestAppEvent appEvent)
        {
            Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, IEnumerable<UserRequestAppEventSubscribeDTO>> result = null;
            lock (lockObj)
            {
                result = queryRunner.Run(new UserRequestAppEventSubscribeQuery(appEvent.RequestEventId, appEvent.Archive, statusRequestMapService.GetEquivalenceByElement));
            }

            if (result != null && result.Item1 != null && result.Item1.Any())
                foreach (var evnt in result.Item1)
                {
                    evnt.BaseUrl = Program.BaseWorkerUrl;
                    foreach (var sender in senders)
                    {
                        await sender.SendAsync(evnt, String.Format(Resource.Subject_RequestAppEventConsumer, evnt.Request.Id, evnt.Request.StatusName), "RequestAppEventWorker");
                        log.InfoFormat("RequestAppEventConsumerHandler Worker Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                            evnt.Request.Id, evnt.Request.StatusName, evnt.Email);
                    }
                }

            if (result != null && result.Item2 != null && result.Item2.Any())
                foreach (var evnt in result.Item2)
                {
                    evnt.BaseUrl = Program.BaseCabinetUrl;
                    foreach (var sender in senders)
                    {
                        await sender.SendAsync(evnt, String.Format(Resource.Subject_RequestAppEventConsumer, evnt.Request.Id, evnt.Request.StatusName), "RequestAppEventCabinet");
                        log.InfoFormat("RequestAppEventConsumerHandler Cabinet Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                            evnt.Request.Id, evnt.Request.StatusName, evnt.Email);
                    }
                }
        }
    }
}