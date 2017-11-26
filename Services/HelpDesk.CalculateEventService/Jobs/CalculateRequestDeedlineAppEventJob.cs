using HelpDesk.CalculateEventService.Query;
using HelpDesk.Common.EventBus.AppEvents;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using HelpDesk.Data.Query;
using MassTransit.Logging;
using Quartz;
using System.Collections.Generic;
using System.Linq;


namespace HelpDesk.CalculateEventService.Jobs
{
    public class CalculateRequestDeedlineAppEventJob : IJob
    {
        private readonly IQueue<IRequestDeedlineAppEvent> queue;
        private readonly IQueryRunner queryRunner;
        private readonly ILog log;
        public CalculateRequestDeedlineAppEventJob(IQueue<IRequestDeedlineAppEvent> queue, IQueryRunner queryRunner, ILog log)
        {
            this.queue = queue;
            this.queryRunner = queryRunner;
            this.log = log;
        }

        public void Execute(IJobExecutionContext context)
        {
            IEnumerable<long> requestIds = queryRunner.Run(new RequestDeedlineQuery());
            if(requestIds == null || !requestIds.Any())
                return;

            queue.Push(new RequestDeedlineAppEvent { RequestIds = requestIds });
            
            log.Info("Push events in bus OK");
        }
    }
}