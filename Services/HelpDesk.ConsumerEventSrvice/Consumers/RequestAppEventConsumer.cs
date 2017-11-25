using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.ConsumerEventSrvice.Consumers.Interface;
using HelpDesk.ConsumerEventSrvice.Query;
using HelpDesk.Data.Query;
using HelpDesk.ConsumerEventSrvice.DTO;
using System.Collections.Generic;
using HelpDesk.ConsumerEventSrvice.Sender;

namespace HelpDesk.ConsumerEventSrvice.Consumers
{
    public class RequestAppEventConsumer : IConsumer<IRequestAppEvent>
    {
        private readonly ILog log;
        private readonly IQueryRunner queryRunner;
        private readonly ISender sender;
        public RequestAppEventConsumer(IQueryRunner queryRunner, IRequestDeedlineAppEventConsumerLog log, ISender sender)
        {
            this.queryRunner = queryRunner;
            this.log = log;
            this.sender = sender;
        }

        public async Task Consume(ConsumeContext<IRequestAppEvent> context)
        {
            await Task.Run(() =>
            {
                log.InfoFormat("RequestAppEventConsumer: RequestEventId = {0}", context.Message.RequestEventId);
                IEnumerable<UserRequestAppEventSubscribeDTO> list =
                    queryRunner.Run(new UserRequestAppEventSubscribeQuery(context.Message.RequestEventId));

                foreach (var evnt in list)
                {
                    sender.Send(evnt, "RequestAppEvent");
                    log.InfoFormat("RequestAppEventConsumer Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                        evnt.RequestId, evnt.RequestStatusName, evnt.Email);
                }
            });
        }        
    }
}