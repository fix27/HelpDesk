using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using System;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;

namespace HelpDesk.ConsumerEventSrvice.Consumers
{
    public class RequestDeedlineAppEventConsumer : IConsumer<IRequestDeedlineAppEvent>
    {
        

        private readonly ILog log = Logger.Get<RequestDeedlineAppEventConsumer>();

        private readonly IBaseRepository<Request> requestRepository;

        public RequestDeedlineAppEventConsumer(IBaseRepository<Request> requestRepository)
        {
            this.requestRepository = requestRepository;
        }


        public async Task Consume(ConsumeContext<IRequestDeedlineAppEvent> context)
        {
            //await log.InfoFormat("RequestId = {0}", context.Message.RequestId);
            int c = requestRepository.Count();
            await Console.Out.WriteLineAsync($"count request: {context.Message.RequestId} {c}");
        }
    }
}