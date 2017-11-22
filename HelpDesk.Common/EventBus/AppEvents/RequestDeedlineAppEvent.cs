namespace HelpDesk.Common.EventBus.AppEvents
{
    public class RequestDeedlineAppEvent : IRequestEvent
    {
        public long RequestEventId { get; set; }
    }
}
