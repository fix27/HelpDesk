using HelpDesk.Common.EventBus.AppEvents.Interface;

namespace HelpDesk.Common.EventBus.Interface
{
    public interface IQueue<T>
        where T: class, IAppEvent 
    {
        void Push(T evnt);
        void Stop();
    }
        
}
