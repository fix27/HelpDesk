using System.Threading.Tasks;
using HelpDesk.EventBus.Common.AppEvents.Interface;

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