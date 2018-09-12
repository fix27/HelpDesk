using HelpDesk.CalculateEventService.Query;
using HelpDesk.EventBus.Common.AppEvents;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.EventBus.Common.Interface;
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
        private readonly IQueryHandler queryHandler;
		private readonly RequestDeedlineQuery _requestDeedlineQuery;
		private readonly ILog log;
        public CalculateRequestDeedlineAppEventJob(IQueue<IRequestDeedlineAppEvent> queue, 
			IQueryHandler queryHandler,
			RequestDeedlineQuery requestDeedlineQuery,
			ILog log)
        {
            this.queue = queue;
            this.queryHandler = queryHandler;
			_requestDeedlineQuery = requestDeedlineQuery;

			this.log = log;
        }

        public void Execute(IJobExecutionContext context)
        {
			var requestIds = queryHandler.Handle<object, IEnumerable<long>, RequestDeedlineQuery>(
				null, _requestDeedlineQuery);
			
			if (requestIds == null || !requestIds.Any())
                return;

            queue.Push(new RequestDeedlineAppEvent { RequestIds = requestIds });
            
            log.Info("Push events in bus OK");
        }
    }
}