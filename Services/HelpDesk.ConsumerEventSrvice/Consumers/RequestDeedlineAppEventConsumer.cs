using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using System;
using HelpDesk.ConsumerEventSrvice.Consumers.Interface;
using HelpDesk.Data.Query;
using HelpDesk.ConsumerEventSrvice.Sender;
using HelpDesk.ConsumerEventSrvice.DTO;
using System.Collections.Generic;
using HelpDesk.ConsumerEventSrvice.Query;
using HelpDesk.ConsumerEventSrvice.Resources;

namespace HelpDesk.ConsumerEventSrvice.Consumers
{
    public class RequestDeedlineAppEventConsumer : IConsumer<IRequestDeedlineAppEvent>
    {
        private readonly ILog log;
        private readonly IQueryRunner queryRunner;
        private readonly ISender sender;
        public RequestDeedlineAppEventConsumer(IQueryRunner queryRunner, IRequestDeedlineAppEventConsumerLog log, 
            ISender sender)
        {
            this.queryRunner = queryRunner;
            this.log = log;
            this.sender = sender;
        }

        public async Task Consume(ConsumeContext<IRequestDeedlineAppEvent> context)
        {
            await Task.Run(() =>
            {
                log.InfoFormat("RequestDeedlineAppEventConsumer: RequestId = {0}", context.Message.RequestId);
                IEnumerable<UserDeedlineAppEventSubscribeDTO> list =
                    queryRunner.Run(new UserRequestDeedlineAppEventSubscribeQuery(context.Message.RequestId));

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