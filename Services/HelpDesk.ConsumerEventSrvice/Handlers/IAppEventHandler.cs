using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.Resources;
using System;

namespace HelpDesk.ConsumerEventService.Handlers
{
    /// <summary>
    /// Базовый класс обработчик события, полученного из шины
    /// </summary>
    public interface IAppEventHandler<T> where T: IAppEvent
    {
        Task Handle(T appEvent);
    }
}