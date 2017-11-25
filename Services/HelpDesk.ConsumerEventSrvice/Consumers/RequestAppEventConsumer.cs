using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.ConsumerEventSrvice.Query;
using HelpDesk.Data.Query;
using HelpDesk.ConsumerEventSrvice.DTO;
using System.Collections.Generic;
using HelpDesk.ConsumerEventSrvice.Sender;
using HelpDesk.ConsumerEventSrvice.Resources;
using System.Linq;

namespace HelpDesk.ConsumerEventSrvice.Consumers
{
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

        public async Task Consume(ConsumeContext<IRequestAppEvent> context)
        {
            log.InfoFormat("RequestAppEventConsumer: RequestEventId = {0}", context.Message.RequestEventId);
            IEnumerable<UserRequestAppEventSubscribeDTO> list =
                queryRunner.Run(new UserRequestAppEventSubscribeQuery(context.Message.RequestEventId));
            if (list == null || !list.Any())
                return;

            await Task.Run(() =>
            {
                foreach (var evnt in list)
                {
                    sender.Send(evnt, String.Format(Resource.Subject_RequestAppEventConsumer, evnt.RequestId, evnt.RequestStatusName), "RequestAppEvent");
                    log.InfoFormat("RequestAppEventConsumer Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                        evnt.RequestId, evnt.RequestStatusName, evnt.Email);
                }
            });
        }        
    }
}