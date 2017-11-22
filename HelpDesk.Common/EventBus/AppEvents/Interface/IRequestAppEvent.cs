namespace HelpDesk.Common.EventBus.AppEvents.Interface
{
    public interface IRequestAppEvent : IAppEvent
    {
        long RequestEventId { get; }
    }
}
