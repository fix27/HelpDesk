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
        private readonly ISender sender;
        public RequestAppEventConsumer(IQueryRunner queryRunner, IStatusRequestMapService statusRequestMapService, ILog log, ISender sender)
        {
            this.queryRunner = queryRunner;
            this.statusRequestMapService = statusRequestMapService;
            this.log = log;
            this.sender = sender;
        }

        static object lockObj = new object();
        public async Task Consume(ConsumeContext<IRequestAppEvent> context)
        {
            log.InfoFormat("RequestAppEventConsumer: RequestEventId = {0}", context.Message.RequestEventId);

            Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>,IEnumerable<UserRequestAppEventSubscribeDTO>> result = null;
            lock (lockObj)
            {
                result = queryRunner.Run(new UserRequestAppEventSubscribeQuery(context.Message.RequestEventId, statusRequestMapService.GetEquivalenceByElement));
            }
            

            var t =  Task.Run(() =>
            {
                if (result != null && result.Item1!=null && result.Item1.Any())
                    foreach (var evnt in result.Item1)
                    {
                        sender.Send(evnt, String.Format(Resource.Subject_RequestAppEventConsumer, evnt.RequestId, evnt.RequestStatusName), "RequestAppEventWorker");
                        log.InfoFormat("RequestAppEventConsumer Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                            evnt.RequestId, evnt.RequestStatusName, evnt.Email);
                    }

                if (result != null && result.Item2 != null && result.Item2.Any())
                    foreach (var evnt in result.Item2)
                    {
                        sender.Send(evnt, String.Format(Resource.Subject_RequestAppEventConsumer, evnt.RequestId, evnt.RequestStatusName), "RequestAppEventCabinet");
                        log.InfoFormat("RequestAppEventConsumer Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                            evnt.RequestId, evnt.RequestStatusName, evnt.Email);
                    }
            });

            await t;
        }        
    }
}