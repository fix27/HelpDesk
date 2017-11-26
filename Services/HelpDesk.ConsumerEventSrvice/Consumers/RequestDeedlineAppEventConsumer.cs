using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using System;
using HelpDesk.Data.Query;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.DTO;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.ConsumerEventService.Query;
using HelpDesk.ConsumerEventService.Resources;

namespace HelpDesk.ConsumerEventService.Consumers
{
    public class RequestDeedlineAppEventConsumer : IConsumer<IRequestDeedlineAppEvent>
    {
        private readonly ILog log;
        private readonly IQueryRunner queryRunner;
        private readonly ISender sender;
        public RequestDeedlineAppEventConsumer(IQueryRunner queryRunner, ILog log, 
            ISender sender)
        {
            this.queryRunner = queryRunner;
            this.log = log;
            this.sender = sender;
        }

        static object lockObj = new object();
        public async Task Consume(ConsumeContext<IRequestDeedlineAppEvent> context)
        {
            log.InfoFormat("RequestDeedlineAppEventConsumer: RequestId = {0}", context.Message.RequestId);

            IEnumerable<UserDeedlineAppEventSubscribeDTO> list = null;
            lock (lockObj)
            {
                list = queryRunner.Run(new UserRequestDeedlineAppEventSubscribeQuery(context.Message.RequestId));
            }
            if (list == null || !list.Any())
                return;
            await Task.Run(() =>
            {
                foreach (var evnt in list)
                {
                    sender.Send(evnt, String.Format(Resource.Subject_RequestDeedlineAppEventConsumer, evnt.RequestId), "RequestDeedlineAppEvent");
                    log.InfoFormat("RequestDeedlineAppEventConsumer Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                        evnt.RequestId, evnt.RequestStatusName, evnt.Email);
                }
            });
        }
    }
}