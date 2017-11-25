using HelpDesk.Common.EventBus.AppEvents;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using Quartz;
using System;

namespace HelpDesk.CalculateEventService.Jobs
{
    public class CalculateRequestDeedlineAppEventJob : IJob
    {
        private readonly IQueue<IRequestDeedlineAppEvent> queue;

        public CalculateRequestDeedlineAppEventJob(IQueue<IRequestDeedlineAppEvent> queue)
        {
            this.queue = queue;
        }

        public void Execute(IJobExecutionContext context)
        {
            queue.Push(new RequestDeedlineAppEvent() { RequestId = 100156 });
            Console.WriteLine("-");
        }
    }
}