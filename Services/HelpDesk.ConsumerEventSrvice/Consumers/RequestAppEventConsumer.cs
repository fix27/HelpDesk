using System;
using System.Threading.Tasks;
using MassTransit;
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


namespace HelpDesk.ConsumerEventService.Consumers
{
    /// <summary>
    /// Получатель события "Изменилось состояние заявки"
    /// </summary>
    public class RequestAppEventConsumer : IConsumer<IRequestAppEvent>
    {
        
        private readonly IQueryRunner queryRunner;
        private readonly IStatusRequestMapService statusRequestMapService;
        private readonly ILog log;
        private readonly IEnumerable<ISender> senders;
        public RequestAppEventConsumer(IQueryRunner queryRunner, IStatusRequestMapService statusRequestMapService, ILog log, IEnumerable<ISender> senders)
        {
            this.queryRunner = queryRunner;
            this.statusRequestMapService = statusRequestMapService;
            this.log = log;
            this.senders = senders;
        }

        static object lockObj = new object();
        public async Task Consume(ConsumeContext<IRequestAppEvent> context)
        {
            log.InfoFormat("RequestAppEventConsumer: RequestEventId = {0}", context.Message.RequestEventId);

            Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>,IEnumerable<UserRequestAppEventSubscribeDTO>> result = null;
            lock (lockObj)
            {
                result = queryRunner.Run(new UserRequestAppEventSubscribeQuery(context.Message.RequestEventId, context.Message.Archive, statusRequestMapService.GetEquivalenceByElement));
            }

            if (result != null && result.Item1 != null && result.Item1.Any())
                foreach (var evnt in result.Item1)
                {
                    evnt.BaseUrl = Program.BaseWorkerUrl;
                    foreach (var sender in senders)
                    {
                        await sender.SendAsync(evnt, String.Format(Resource.Subject_RequestAppEventConsumer, evnt.Request.Id, evnt.Request.StatusName), "RequestAppEventWorker");
                        log.InfoFormat("RequestAppEventConsumer Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
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
                        log.InfoFormat("RequestAppEventConsumer Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                            evnt.Request.Id, evnt.Request.StatusName, evnt.Email);
                    }
                }
        }        
    }
}