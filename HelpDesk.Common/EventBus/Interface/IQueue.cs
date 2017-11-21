using HelpDesk.Common.EventBus.AppEvents;

namespace HelpDesk.Common.EventBus.Interface
{
    public interface IQueue
    {
        void Push(IAppEvent evnt);
    }
}
