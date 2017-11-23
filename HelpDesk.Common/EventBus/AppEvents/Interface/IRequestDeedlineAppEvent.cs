namespace HelpDesk.Common.EventBus.AppEvents.Interface
{
    public interface IRequestDeedlineAppEvent : IAppEvent
    {
        long RequestId { get; }
    }
}
