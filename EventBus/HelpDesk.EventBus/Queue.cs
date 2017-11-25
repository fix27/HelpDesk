using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using MassTransit;

namespace HelpDesk.EventBus
{
    public class Queue<T>: IQueue<T>
        where T : class, IAppEvent 
    {
        private readonly IBusControl bus;
        
        public Queue(IBusControl bus)
        {
            this.bus = bus;
        }

        public void Push(T evnt)
        {
            bus.Publish<T>(evnt);
        }
    }
}
