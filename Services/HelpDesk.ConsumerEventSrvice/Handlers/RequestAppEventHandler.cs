using System;
using System.Threading.Tasks;
using MassTransit.Logging;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.ConsumerEventService.Query;
using HelpDesk.Data.Query;
using HelpDesk.ConsumerEventService.DTO;
using System.Collections.Generic;
using HelpDesk.ConsumerEventService.Sender;
using System.Linq;
using HelpDesk.DataService.Common.Interface;


namespace HelpDesk.ConsumerEventService.Handlers
{
    /// <summary>
    /// Обработчик события "Изменилось состояние заявки"
    /// </summary>
    public class RequestAppEventHandler : IAppEventHandler<IRequestAppEvent>
    {

        private readonly IQueryHandler queryHandler;
		private readonly UserRequestAppEventSubscribeQuery _userRequestAppEventSubscribeQuery;
		private readonly IStatusRequestMapService statusRequestMapService;
        private readonly ILog log;
        private readonly IEnumerable<ISender> senders;
        public RequestAppEventHandler(IQueryHandler queryHandler,
			UserRequestAppEventSubscribeQuery userRequestAppEventSubscribeQuery,
			IStatusRequestMapService statusRequestMapService, 
			ILog log, 
			IEnumerable<ISender> senders)
        {
            this.queryHandler = queryHandler;
			_userRequestAppEventSubscribeQuery = userRequestAppEventSubscribeQuery;
			this.statusRequestMapService = statusRequestMapService;
            this.log = log;
            this.senders = senders;
        }

        static object lockObj = new object();
        public async Task Handle(IRequestAppEvent appEvent)
        {
            Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, IEnumerable<UserRequestAppEventSubscribeDTO>> result = null;
            lock (lockObj)
            {
                result = queryHandler.Handle<UserRequestAppEventSubscribeQueryParam,
					Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, IEnumerable<UserRequestAppEventSubscribeDTO>>,
					UserRequestAppEventSubscribeQuery>(
					new UserRequestAppEventSubscribeQueryParam
					{
						 RequestEventId = appEvent.RequestEventId,
						 Archive = appEvent.Archive,
						 GetEquivalenceByElement = statusRequestMapService.GetEquivalenceByElement
					},
					_userRequestAppEventSubscribeQuery);
            }

            if (result != null && result.Item1 != null && result.Item1.Any())
                foreach (var evnt in result.Item1)
                {
                    evnt.BaseUrl = Program.BaseWorkerUrl;
                    foreach (var sender in senders)
                    {
                        await sender.SendAsync(evnt);
                        log.InfoFormat("RequestAppEventConsumerHandler Worker Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                            evnt.Request.Id, evnt.Request.StatusName, evnt.Email);
                    }
                }

            if (result != null && result.Item2 != null && result.Item2.Any())
                foreach (var evnt in result.Item2)
                {
                    evnt.BaseUrl = Program.BaseCabinetUrl;
                    foreach (var sender in senders)
                    {
                        await sender.SendAsync(evnt);
                        log.InfoFormat("RequestAppEventConsumerHandler Cabinet Send OK: RequestId = {0}, RequestStatusName = {1}, Email = {2}",
                            evnt.Request.Id, evnt.Request.StatusName, evnt.Email);
                    }
                }
        }
    }
}