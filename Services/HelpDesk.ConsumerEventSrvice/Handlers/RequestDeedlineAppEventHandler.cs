using System.Threading.Tasks;
using MassTransit.Logging;
using HelpDesk.Data.Query;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.DTO;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.ConsumerEventService.Query;
using HelpDesk.EventBus.Common.AppEvents.Interface;

namespace HelpDesk.ConsumerEventService.Handlers
{
    /// <summary>
    /// Обработчик события "Истекает срок по заявке"
    /// </summary>
    public class RequestDeedlineAppEventHandler : IAppEventHandler<IRequestDeedlineAppEvent>
    {
        private readonly ILog log;
        private readonly IQueryHandler queryHandler;
		private readonly UserRequestDeedlineAppEventSubscribeQuery _userRequestDeedlineAppEventSubscribeQuery;
		private readonly IEnumerable<ISender> senders;
        public RequestDeedlineAppEventHandler(IQueryHandler queryHandler,
			UserRequestDeedlineAppEventSubscribeQuery userRequestDeedlineAppEventSubscribeQuery,
			ILog log,
            IEnumerable<ISender> senders)
        {
            this.queryHandler = queryHandler;
			_userRequestDeedlineAppEventSubscribeQuery = userRequestDeedlineAppEventSubscribeQuery;
			this.log = log;
            this.senders = senders;
        }

        static object lockObj = new object();
        public async Task Handle(IRequestDeedlineAppEvent appEvent)
        {
            IEnumerable<UserDeedlineAppEventSubscribeDTO> list = null;
            lock (lockObj)
            {
                list = queryHandler.Handle<UserRequestDeedlineAppEventSubscribeQueryParam, 
					IEnumerable<UserDeedlineAppEventSubscribeDTO>, UserRequestDeedlineAppEventSubscribeQuery>
					(new UserRequestDeedlineAppEventSubscribeQueryParam
					{
						 RequestIds = appEvent.RequestIds
					}, _userRequestDeedlineAppEventSubscribeQuery);
            }
            log.InfoFormat("-----------------------------: list.Count = {0}", list.Count());
            if (list == null || !list.Any())
                return;
            foreach (var evnt in list)
            {
                evnt.BaseUrl = Program.BaseWorkerUrl;
                foreach (var sender in senders)
                {
                    await sender.SendAsync(evnt);
                    log.InfoFormat("RequestDeedlineAppEventHandler Send OK: Email = {0}", evnt.Email);
                }
            }
        }
    }
}