namespace HelpDesk.Common.EventBus.AppEvents
{
    public class RequestAppEvent: IRequestEvent
    {
        public long RequestEventId { get; set; }
    }
}
