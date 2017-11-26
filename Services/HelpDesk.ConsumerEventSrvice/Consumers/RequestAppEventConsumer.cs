using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.ConsumerEventService.Query;
using HelpDesk.Data.Query;
using HelpDesk.ConsumerEventService.DTO;
using System.Collections.Generic;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.Resources;
using System.Linq;

namespace HelpDesk.ConsumerEventService.Consumers
{
    /// <summary>
    /// Получатель события "Изменилось состояние заявки"
    /// </summary>
    public class RequestAppEventConsumer : IConsumer<IRequestAppEvent>
    {
        private readonly ILog log;
        private readonly IQueryRunner queryRunner;
        private readonly ISender sender;
        public RequestAppEventConsumer(IQueryRunner queryRunner, ILog log, ISender sender)
        {
            this.queryRunner = queryRunner;
            this.log = log;
            this.sender = sender;
        }

        static object lockObj = new object();
        public async Task Consume(ConsumeContext<IRequestAppEvent> context)
        {
            log.InfoFormat("RequestAppEventConsumer: RequestEventId = {0}", context.Message.RequestEventId);

            IEnumerable<UserRequestAppEventSubscribeDTO> list = null;
            lock (lockObj)
            {
                list = queryRunner.Run(new UserRequestAppEventSubscribeQuery(context.Message.RequestEventId));
            }
            
            if (list == null || !list.Any())
                return;

            var t =  Task.Run(() =>
            {
                foreach (var evnt in list)
                {
                    sender.Send(evnt, String.Format(Resource.Subject_RequestAppEventConsumer, evnt.RequestId, evnt.RequestStatusName), "RequestAppEvent");
                    log.InfoFormat("RequestAppEventConsumer Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                        evnt.RequestId, evnt.RequestStatusName, evnt.Email);
                }
            });

            await t;
        }        
    }
}